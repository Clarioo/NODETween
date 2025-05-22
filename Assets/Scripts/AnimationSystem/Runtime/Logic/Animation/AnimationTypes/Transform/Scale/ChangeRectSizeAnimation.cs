namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Transform.Scale
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Transform;
    using Graph.Animations.CreationTools.ParameterTypes;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;

    [Serializable]
    public class ChangeRectSizeAnimation : IAnimable
    {
        // Rect Size components and parameters
        [Header("Rect Size")]
        [SerializeField, Tooltip("Object you want to change size")]
        private RectTransform objectToScale;
        [SerializeField, Tooltip("Target Size")]
        private Vector2ParameterData targetSize;

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
            var node = BaseNode.CreateFromType<ChangeRectSizeAnimationNode>(position);
            node.ChangeRectSizeAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
            return node;
        }
        
        public Type GetAnimableType()
        {
            return typeof(RectTransform);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToScale = gameObject.GetComponent<RectTransform>();
        }

        public Tween GetTween()
        {
            return objectToScale.DOSizeDelta(targetSize.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetTargetSize(Vector2ParameterData size)
        {
            targetSize = size;
        }
    }
}
