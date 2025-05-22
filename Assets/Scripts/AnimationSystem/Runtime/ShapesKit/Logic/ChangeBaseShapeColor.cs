namespace AnimationSystem.Runtime.ShapesKit.Logic
{
    using _3rdParty.ThisOtherThing.UI_Shapes_Kit.Geometry;
    using AnimationSystem.Runtime.Graph.Animations;
    using AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Rendering;
    using AnimationSystem.Runtime.Graph.Animations.CreationTools.ParameterTypes;
    using AnimationSystem.Runtime.Logic.Animation;
    using AnimationSystem.Runtime.Logic.Animation.Interfaces;
    using DG.Tweening;
    using Graph;
    using GraphProcessor;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ChangeBaseShapeColor : IAnimable
    {
        public enum ShapeColorChangeType
        {
            Fill,
            Outline
        }
        
// Color components and parameters
        [Header("Color")]
        [SerializeField]
        private IShapeFill graphicToChangeColor;
        [SerializeField]
        private ColorParameterData targetColor;
        
        [Header("Shape")]
        [SerializeField]
        private ShapeType shapeType;
        [SerializeField]
        private ShapeColorChangeType shapeColorChangeType;
        
        // Main animation config
        public SequenceAddType SequenceAddType { get; private set; }

        [field: SerializeField]
        public float Delay { get; private set; }

        [field: SerializeField]
        public int Loops { get; private set; }

        [field: SerializeField]
        public float AnimationTime { get; private set; }

        [field: SerializeField]
        public Ease Ease { get; private set; }

        [field: SerializeField, HideInInspector]
        public string AssignedNodeGUID { get; set; }

        public AnimationNode CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            var node = BaseNode.CreateFromType<ShapeColorChangeAnimationNode>(position);
            node.ChangeShapeColorAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
            return node;
        }

        public Type GetAnimableType()
        {
            return ShapesMap.ShapesMapDictionary[shapeType];
        }

        public Tween GetTween()
        {
            Tween tween = null;
            switch (shapeColorChangeType)
            {
                case ShapeColorChangeType.Fill:
                    tween = DOTween
                        .To(() => graphicToChangeColor.ShapeFillProperties.FillColor,
                            x => graphicToChangeColor.ShapeFillProperties.FillColor = x, targetColor.ParameterValue,
                            AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops).OnUpdate(graphicToChangeColor.ForceMeshUpdate);;
                    break;
                case ShapeColorChangeType.Outline:
                    var outlineProperties = graphicToChangeColor.ShapeFillProperties as GeoUtils.OutlineShapeProperties;
                    tween = DOTween
                        .To(() => outlineProperties.OutlineColor,
                            x => outlineProperties.OutlineColor = x, targetColor.ParameterValue,
                            AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops).OnUpdate(graphicToChangeColor.ForceMeshUpdate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return tween;
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            var shapeComponentType = ShapesMap.ShapesMapDictionary[shapeType];
            graphicToChangeColor = gameObject.GetComponent(shapeComponentType) as IShapeFill;
        }

        public void SetTargetColor(ColorParameterData parameter)
        {
            targetColor = parameter;
        }
    }
}