namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Transform.Position
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Transform;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;

    [Serializable]
    public class PathAnimation : IAnimable
    {
        // Path components and parameters
        [Header("Path")]
        [SerializeField, Tooltip("Layout Object you want to move with specified path")]
        private Transform objectToMove;
        [SerializeField, Tooltip("List of points that object is going to move trough")]
        private Vector3[] movingPath = new Vector3[1];
        [SerializeField, Tooltip("Path movement type")]
        private PathType pathType = PathType.Linear;

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
            var node = BaseNode.CreateFromType<PathAnimationNode>(position);
            node.PathAnimation = this;
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

        public Tween GetTween()
        {
            return objectToMove.DOLocalPath(movingPath, AnimationTime, pathType).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToMove = gameObject.GetComponent<Transform>();
        }
    }
}