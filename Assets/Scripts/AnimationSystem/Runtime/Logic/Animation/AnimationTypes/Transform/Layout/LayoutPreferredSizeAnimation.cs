namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Transform.Layout
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Transform;
    using Graph.Animations.CreationTools.ParameterTypes;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class LayoutPreferredSizeAnimation : IAnimable
    {
        // Layout components and parameters
        [Header("Layout")]
        [SerializeField, Tooltip("Layout Object you want to scale on Canvas")]
        private LayoutElement objectToScale;

        [SerializeField,
         Tooltip("Object target layoutSize position at the end of animation")]
        private Vector2ParameterData targetLayoutSize;

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
            var node = BaseNode.CreateFromType<LayoutPreferredSizeAnimationNode>(position);
            node.LayoutPreferredSizeAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
            return node;
        }

        public Type GetAnimableType()
        {
            return typeof(LayoutElement);
        }

        public Tween GetTween()
        {
            return objectToScale.DOPreferredSize(targetLayoutSize.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToScale = gameObject.GetComponent<LayoutElement>();
        }

        public void SetTargetLayoutSize(Vector2ParameterData targetSize)
        {
            targetLayoutSize = targetSize;
        }
    }
}