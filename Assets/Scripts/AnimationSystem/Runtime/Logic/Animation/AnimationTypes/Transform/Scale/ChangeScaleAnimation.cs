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
    public class ChangeScaleAnimation : IAnimable
    {
        // Scale components and parameters
        [Header("Scale")]
        [SerializeField, Tooltip("Object you want to change size")]
        private Transform objectToScale;
        [SerializeField, Tooltip("Target Scale")]
        private Vector3ParameterData targetScale;

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
            var node = BaseNode.CreateFromType<ScaleAnimationNode>(position);
            node.ChangeScaleAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
            return node;
        }
        
        public Type GetAnimableType()
        {
            return typeof(Transform);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToScale = gameObject.GetComponent<Transform>();
        }

        public Tween GetTween()
        {
            return objectToScale.DOScale(targetScale.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetTargetScale(Vector3ParameterData scale)
        {
            targetScale = scale;
        }
    }
}
