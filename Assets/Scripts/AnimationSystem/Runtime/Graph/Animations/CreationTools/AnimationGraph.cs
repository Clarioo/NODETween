namespace AnimationSystem.Runtime.Graph.Animations.CreationTools
{
    using System.Collections.Generic;
    using System.Linq;
    using GraphProcessor;
    using UnityEngine;

    [System.Serializable, CreateAssetMenu(fileName = "Animation Graph", menuName = "Graphs/Animation Graph", order = 0)]
    public class AnimationGraph : BaseGraph
    {
        public List<AnimationNode> GraphAnimationNodes => GetAnimationNodes();
        
        public List<AnimationNode> GetAnimationNodes()
        {
            List<AnimationNode> animationNodes = new List<AnimationNode>();
            foreach(var baseNode in nodes)
            {
                if(baseNode.GetType().IsSubclassOf(typeof(AnimationNode)))
                {
                    animationNodes.Add(baseNode as AnimationNode);
                }
            }
            return animationNodes;
        }

        public List<T> GetNodesOfType<T>() where T : BaseNode
        {
            List<T> typeNodes = new List<T>();
            foreach (var baseNode in nodes)
            {
                if (baseNode.GetType() == typeof(T) )
                {
                    typeNodes.Add(baseNode as T);
                }
            }
            return typeNodes;
        }

        public List<ParameterNode> GetParameterNodesOfType<T>()
        {
            List<ParameterNode> typeNodes = new List<ParameterNode>();
            var paramNodes = GetNodesOfType<ParameterNode>();
            foreach (var paramNode in paramNodes)
            {
                if (paramNode.parameter.GetValueType() == typeof(T))
                {
                    typeNodes.Add(paramNode);
                }
            }
            return typeNodes;
        }

        public List<T> GetParametersOfType<T>() where T : ExposedParameter
        {
            List<T> list = new List<T>();
            foreach(var val in exposedParameters)
            {
                if (val.GetType() == typeof(T))
                {
                    list.Add(val as T);
                }
            }
            return list;
        }

        public int GetFirstFreeId()
        {
            List<int> ids = GetAnimationNodes().Select(x => x.NodeId).ToList();
            int i = 0;
            while (ids.Contains(i))
            {
                i++;
            }
            return i;
        }
    }
}