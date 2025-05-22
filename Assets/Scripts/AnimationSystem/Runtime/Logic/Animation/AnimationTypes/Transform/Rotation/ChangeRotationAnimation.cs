namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Transform.Rotation
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
    public class ChangeRotationAnimation : IAnimable
    {
        // Rotation components and parameters
        [Header("Rotation")]
        [SerializeField, Tooltip("Object you want to change rotation")]
        private Transform objectToRotate;
        [SerializeField, Tooltip("Target Rotation in euler angles")]
        private Vector3ParameterData targetEulersRot;
        [SerializeField, Tooltip("If TRUE = rotation value is target value. If FALSE - target value is rotation angle")]
        private bool useToRotation = true;

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
            var node = BaseNode.CreateFromType<RotateAnimationNode>(position);
            node.ChangeRotationAnimation = this;
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
            return !useToRotation 
                ? objectToRotate.DOLocalRotate(targetEulersRot.ParameterValue, AnimationTime, RotateMode.Fast).SetDelay(Delay).SetEase(Ease).SetLoops(Loops) 
                : objectToRotate.DOLocalRotate(targetEulersRot.ParameterValue, AnimationTime, RotateMode.LocalAxisAdd).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToRotate = gameObject.GetComponent<Transform>();
        }

        public void SetTargetRotation(Vector3ParameterData rot)
        {
            targetEulersRot = rot;
        }
    }
}