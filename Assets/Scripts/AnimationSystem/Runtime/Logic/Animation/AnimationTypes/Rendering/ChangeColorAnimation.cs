namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Rendering
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Rendering;
    using Graph.Animations.CreationTools.ParameterTypes;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ChangeColorAnimation : IAnimable
    {
        // Color components and parameters
        [Header("Color")]
        [SerializeField]
        private MaskableGraphic graphicToChangeColor;
        [SerializeField]
        private ColorParameterData targetColor;

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
            var node = BaseNode.CreateFromType<ColorChangeAnimationNode>(position);
            node.ChangeColorAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
            return node;
        }

        public Type GetAnimableType()
        {
            return typeof(MaskableGraphic);
        }

        public Tween GetTween()
        {
            return graphicToChangeColor.DOColor(targetColor.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            graphicToChangeColor = gameObject.GetComponent<MaskableGraphic>();
        }
        
        public void SetTargetColor(ColorParameterData parameter)
        {
            targetColor = parameter;
        }
    }

}
