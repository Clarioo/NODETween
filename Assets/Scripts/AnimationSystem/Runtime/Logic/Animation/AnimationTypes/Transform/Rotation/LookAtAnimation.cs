namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Transform.Rotation
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Transform;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;

    [Serializable]
    public class LookAtAnimation : IAnimable
    {
        // LookAt components and parameters
        [Header("LookAt")]
        [SerializeField, Tooltip("Object you want to change rotation")]
        private Transform objectToRotate;
        [SerializeField, Tooltip("Defines rotation source. If TRUE animation will use goToLookAt as source, if FALSE - lookAtPosition")]
        private bool useObject;
        [SerializeField, Tooltip("Position that objectToRotate is going to look at")]
        private Vector3 lookAtPosition;
        [SerializeField, Tooltip("Object that objectToRotate is going to look at")]
        private GameObject goToLookAt;

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
            var node = BaseNode.CreateFromType<LookAtAnimationNode>(position);
            node.LookAtAnimation = this;
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
            var tweenParams = new TweenParams().SetDelay(Delay).SetEase(Ease).SetLoops(Loops);

            if (useObject)
            {

                return objectToRotate.DODynamicLookAt(goToLookAt.transform.position, AnimationTime).SetAs(tweenParams);
            }
            else
            {
                return objectToRotate.DODynamicLookAt(lookAtPosition, AnimationTime).SetAs(tweenParams);
            }
        }

        public void LookAtPosition()
        {
            objectToRotate.LookAt(goToLookAt.transform);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToRotate = gameObject.GetComponent<Transform>();
        }

        public void SetGOToLookAt(GameObject gameObject)
        {
            goToLookAt = gameObject;
        }
    }
}