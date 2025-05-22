namespace AnimationSystem.Runtime.Graph.Animations.CreationTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #if UNITY_EDITOR
    using Editor.Windows;
    using UnityEditor;
    #endif
    using GraphProcessor;
    using Logic.Animation;
    using ParameterTypes;
    #if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    #endif
    using UnityEngine;
    using Utils.GUILogger;

    [Serializable]
    public class AnimationGraphCreator : IGUILogInvoker
    {
        #region Odin Inspector
        public const string ANIMABLES = "Animables";
        public const string PARAMETERS = "Parameters";
        public const string GRAPH_CONTROL = "GRAPH CONTROL";
        public const string MATCHING = "MATCHING";
        #endregion


        #region Animation Objects
        #if ODIN_INSPECTOR
        [BoxGroup(ANIMABLES)]
        #endif
        [SerializeField]
        public List<AnimableObject> AnimableObjects;

        #if ODIN_INSPECTOR
        [BoxGroup(PARAMETERS)]
        #endif
        [SerializeField]
        public ParametersContainer ParametersContainer;

        #if ODIN_INSPECTOR
        [BoxGroup(GRAPH_CONTROL)]
  #endif
        [SerializeField]
        public AnimationGraph SampleGraph;
        #endregion

        #region GUILogger
        public List<GUILogData> Logs { get; } = new List<GUILogData>();
  #endregion


#if UNITY_EDITOR

        #region Graph Control
        #if ODIN_INSPECTOR
        [BoxGroup(GRAPH_CONTROL), Button("CreateGraph")]
        #endif
        [Tooltip("Creates graph based on data from AnimableObjects and ParametersContainer")]
        public void CreateGraphFromEditor()
        {
            CreateGraph();
        }

        /// <summary>
        /// Creates animables and parameters using data from animation runner, then shows created graph.
        /// Note that SampleGraph need to be filled with empty animation graph.
        /// </summary>
        public void CreateGraph()
        {
            if (SampleGraph == null)
            {
                this.Log(LogType.Error, "Create empty Animation Graph and assign it to SampleGraph");
                return;
            }

            if (SampleGraph.nodes.Find(n => n.GetType() == typeof(AnimationStartScript)) == null)
            {
                var startNode = BaseNode.CreateFromType<AnimationStartScript>(new Vector2(-200, 0));
                SampleGraph.AddNode(startNode);
            }


            if (UpdateGraph())
            {
                EditorUtility.SetDirty(SampleGraph);
                AssetDatabase.SaveAssetIfDirty(SampleGraph);

                ShowGraph();
            }
        }

        #if ODIN_INSPECTOR
        [BoxGroup(GRAPH_CONTROL), Button("Update Graph")]
        #endif
        [Tooltip("Updates graph based on data from AnimableObjects and ParametersContainer")]
        public void UpdateGraphFromEditor()
        {
            UpdateGraph();
        }

        /// <summary>
        /// Updates current opened graph. Creates graph parameters based on animation runner parameters if they are not existing. 
        /// If AnimationRunner has defined animations in Animables - create nodes with given anim type with attached object nodes.
        /// </summary>
        /// <returns>TRUE if Graph updated correctly. False if updating failed - check logs.</returns>
        public bool UpdateGraph()
        {
            if (SampleGraph == null)
            {
                this.Log(LogType.Error, "Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            for (int i = 0; i < AnimableObjects.Count; i++)
            {
                if (AnimableObjects[i].ObjectToAnimate == null)
                {
                    this.Log(LogType.Error,
                        $"Add Object to {AnimableObjects[i].GraphParameterName} animable and match graph again");
                    return false;
                }

                var parameterGuid = "";
                if (SampleGraph.GetExposedParameter(AnimableObjects[i].GraphParameterName) != null)
                {
                    parameterGuid = SampleGraph.GetExposedParameter(AnimableObjects[i].GraphParameterName).guid;
                }
                else
                {
                    var parameterName = AnimableObjects[i].GraphParameterName != ""
                        ? AnimableObjects[i].GraphParameterName
                        : AnimableObjects[i].ObjectToAnimate.name;
                    parameterGuid = SampleGraph.AddExposedParameter(parameterName, typeof(GameObjectParameter),
                        AnimableObjects[i].ObjectToAnimate);
                    AnimableObjects[i].GraphParameterName = parameterName;
                    EditorUtility.SetDirty(SampleGraph);
                    AssetDatabase.SaveAssetIfDirty(SampleGraph);
                }

                if (AnimableObjects[i].ObjectAnimator == null)
                {
                    AnimableObjects[i].GenerateAnimator();
                }

                var animables = AnimableObjects[i].ObjectAnimator.Animables;
                for (int j = 0; j < animables.Count; j++)
                {
                    var matchedNode = SampleGraph.nodes.Find(n => n.GUID == animables[j].AssignedNodeGUID);
                    if (matchedNode != null)
                    {
                        this.Log(LogType.Warning, $"Node {matchedNode} already exist");
                    }
                    else
                    {
                        var vect = new Vector2(j * 430, -i * 300);
                        var goParameter = BaseNode.CreateFromType<ParameterNode>(vect - new Vector2(80, -70));
                        goParameter.accessor = ParameterAccessor.Get;
                        goParameter.parameterGUID = parameterGuid;
                        SampleGraph.AddNode(goParameter);
                        var node = animables[j].CreateNode(SampleGraph, vect, goParameter);
                        node.NodeId = SampleGraph.GetFirstFreeId();
                    }
                }

                EditorUtility.SetDirty(SampleGraph);
                AssetDatabase.SaveAssetIfDirty(SampleGraph);
            }

            if (!CreateParams())
                return false;

            return true;
        }

        #if ODIN_INSPECTOR
        [BoxGroup(GRAPH_CONTROL), Button("Show Graph")]
        #endif
        [Tooltip("Shows graph in Graph Window")]
        public void ShowGraphFromEditor()
        {
            ShowGraph();
        }

        /// <summary>
        /// Opens current attached Graph if its not null.
        /// </summary>
        /// <returns>TRUE if graph opens correctly, FALSE if graph is not set</returns>
        public bool ShowGraph()
        {
            if (SampleGraph == null)
            {
                this.Log(LogType.Error, "Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            EditorWindow.GetWindow<ExposedPropertiesGraphWindow>()
                .InitializeGraph(SampleGraph as BaseGraph);
            return true;
        }

        /// <summary>
        /// Creates Parameters in graph based on params in AnimationRunner
        /// </summary>
        /// <returns></returns>
        private bool CreateParams()
        {
            if (!CreateParameters<FloatParameterData, float, FloatParameter>(ParametersContainer.FloatParameterDatas))
                return false;
            if (!CreateParameters<Vector3ParameterData, Vector3, Vector3Parameter>(ParametersContainer
                .Vector3ParameterDatas))
                return false;
            if (!CreateParameters<Vector2ParameterData, Vector2, Vector2Parameter>(ParametersContainer
                .Vector2ParameterDatas))
                return false;
            if (!CreateParameters<StringParameterData, string, StringParameter>(
                ParametersContainer.StringParameterDatas))
                return false;
            if (!CreateParameters<ColorParameterData, Color, ColorParameter>(
                ParametersContainer.ColorParameterDatas))
                return false;
            return true;
        }

        private bool CreateParameters<T, TU, TW>(List<T> parameters) where T : BaseParameterData<TU, TW>
            where TW : ExposedParameter, new()
        {
            if (SampleGraph == null)
            {
                this.Log(LogType.Error, "Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].ParameterName == "")
                {
                    this.Log(LogType.Error, $"Empty parameter name on index {i}. Fix it and try again");
                    return false;
                }

                if (SampleGraph.GetExposedParameter(parameters[i].ParameterName) == null)
                {
                    var parameterName = parameters[i].ParameterName;
                    SampleGraph.AddExposedParameter(parameterName, typeof(TW), parameters[i].ParameterValue);
                    EditorUtility.SetDirty(SampleGraph);
                    AssetDatabase.SaveAssetIfDirty(SampleGraph);
                }
            }

            return true;
        }
        #endregion

#endif

        #region Matching
        #if ODIN_INSPECTOR
        [BoxGroup(MATCHING), Button("Check Configuration")]
  #endif
        [Tooltip("Checks if Animation Runner has all needed Animables and Paramaters required by Graph, if not - adds them")]
        public void CheckIfObjectsMatchingGraphParametersFromEditor()
        {
            if (CheckIfObjectsMatchingGraphParameters())
            {
                this.Log(LogType.Log, $"Graph has been configured properly");
            }
        }

        /// <summary>
        /// Checks if Animation Runner has all needed Animables and Paramaters required by Graph
        /// </summary>
        /// <returns>TRUE if has all parameters, FALSE if doesn't have parameters or parameters got empty values.</returns>
        public bool CheckIfObjectsMatchingGraphParameters()
        {
            if (SampleGraph == null)
            {
                this.Log(LogType.Error, "Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            // Check if all nodes are properly set
            var animationNodes = SampleGraph.GetAnimationNodes();
            if (animationNodes.Any(animationNode => !animationNode.AreAllNodesProperlyConfigured()))
            {
                return false;
            }

            // Check if all parameters are set 
            var goNodes = SampleGraph.GetParametersOfType<GameObjectParameter>();

            for (var i = 0; i < goNodes.Count; i++)
            {
                var matched = AnimableObjects.Find(a => a.GraphParameterName == goNodes[i].name);
                if (matched == null)
                {
                    AnimableObjects.Add(new AnimableObject()
                    {
                        GraphParameterName = goNodes[i].name
                    });
                    this.Log(LogType.Error, $"Add Object to {goNodes[i].name} animable and match graph again");
                    return false;
                }
                else
                {
                    if (matched.ObjectToAnimate == null)
                    {
                        this.Log(LogType.Error, $"Add Object to {goNodes[i].name} animable and match graph again");
                        return false;
                    }

                    if (matched.ObjectAnimator == null)
                    {
                        matched.GenerateAnimator();
                    }

                    var nodesAttachedTo = GetNodesConnectedTo(goNodes[i].name);
                    foreach (var node in nodesAttachedTo)
                    {
                        matched.AddComponentWithType(node.GetNeededType());
                        // no need to set animable object here, because it will be set in FillParameters
                        //node.SetAnimableObject((GameObject)node.GetAssignedParameter().parameter.value);
                        if (!matched.ObjectAnimator.Animables.Exists(a => a.AssignedNodeGUID == node.GUID))
                        {
                            matched.ObjectAnimator.Animables.Add(node.Animable);
                        }
                    }
                }
            }

            if (!CheckParametersWithType<Single, FloatParameter, FloatParameterData>(
                ref ParametersContainer.FloatParameterDatas))
                return false;
            if (!CheckParametersWithType<Vector2, Vector2Parameter, Vector2ParameterData>(
                ref ParametersContainer.Vector2ParameterDatas))
                return false;
            if (!CheckParametersWithType<Vector3, Vector3Parameter, Vector3ParameterData>(
                ref ParametersContainer.Vector3ParameterDatas))
                return false;
            if (!CheckParametersWithType<String, StringParameter, StringParameterData>(
                ref ParametersContainer.StringParameterDatas))
                return false;
            if (!CheckParametersWithType<Color, ColorParameter, ColorParameterData>(
                ref ParametersContainer.ColorParameterDatas))
                return false;

            return true;
        }

        /// <summary>
        /// Cheks if Runner has all needed Parameters with given Type.
        /// </summary>
        /// <typeparam name="T">Input value type</typeparam>
        /// <typeparam name="TU">Parameter Type - inherits from ExposedParameter</typeparam>
        /// <typeparam name="W">Parameter Data Model - inherits from BaseParameterData</typeparam>
        /// <param name="paramsInObject">Parameters list to check and fill</param>
        /// <returns>true if fill</returns>
        public bool CheckParametersWithType<T, TU, W>(ref List<W> paramsInObject) where W : BaseParameterData<T, TU>
            where TU : ExposedParameter, new()
        {
            var paramNodes = SampleGraph.GetParametersOfType<TU>();

            foreach (var t in paramNodes)
            {
                var matched = paramsInObject.Find(a => a.ParameterName == t.name);
                if (matched == null)
                {
                    var newParam = (W)Activator.CreateInstance(typeof(W), new object[]
                    {
                        t.name
                    });
                    paramsInObject.Add(newParam);
                }
            }

            return true;
        }
        #endregion

        #region Pre-Animation Filling
        /// <summary>
        /// Fills nodes values in animation graph.
        /// </summary>
        /// <returns>TRUE if values are filled correctly, FALSE if got an error - Check logs</returns>
        public bool FillParameters()
        {
            FillAnimationParameters();

            if (SampleGraph == null)
            {
                this.Log(LogType.Error, "Create empty Animation Graph and assign it to SampleGraph");
                return false;
            }

            var animableParameters = SampleGraph.exposedParameters.FindAll(p => p.GetValueType() == typeof(GameObject));
            if (AnimableObjects.Count < animableParameters.Count)
            {
                this.Log(LogType.Error, $"MISSING OBJECTS! GRAPH NEED {animableParameters.Count} ANIMABLE OBJECTS");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Filling parameters values in Animation Graph based on values from AnimtationRunner
        /// </summary>
        private void FillAnimationParameters()
        {
            var goParameters = new List<GameObjectParameterData>();
            foreach (var go in AnimableObjects)
            {
                var newGoParameter = new GameObjectParameterData(go.GraphParameterName);
                newGoParameter.ParameterValue = go.ObjectToAnimate;
                goParameters.Add(newGoParameter);
            }

            FillParametersOfType<GameObject, GameObjectParameter, GameObjectParameterData>(ref goParameters);
        }

        /// <summary>
        /// Fills parameters of given type T
        /// </summary>
        /// <typeparam name="T">Input value type</typeparam>
        /// <typeparam name="TU">Parameter Type - inherits from ExposedParameter</typeparam>
        /// <typeparam name="TW">Parameter Data Model - inherits from BaseParameterData</typeparam>
        /// <param name="paramsInObject"></param>
        private void FillParametersOfType<T, TU, TW>(ref List<TW> paramsInObject) where TW : BaseParameterData<T, TU>
            where TU : ExposedParameter, new()
        {
            var parameters = SampleGraph.GetParametersOfType<TU>();
            foreach (var parameter in parameters)
            {
                parameter.value = paramsInObject.Find(p => p.ParameterName == parameter.name).ParameterValue;
                SampleGraph.NotifyExposedParameterValueChanged(parameter);
            }
        }
        #endregion

        #region Nodes Finding
        /// <summary>
        /// Gets connected nodes to node with <paramref name="parameterNode"/>
        /// </summary>
        /// <param name="parameterNode">Name of the node</param>
        /// <returns>Connected nodes</returns>
        public List<AnimationNode> GetNodesConnectedTo(string parameterNode)
        {
            var animNodes = SampleGraph.GraphAnimationNodes;
            var connectedTo = animNodes.FindAll(n => n.GetAssignedParameter().parameter.name == parameterNode);
            return connectedTo;
        }
        #endregion
    }
}