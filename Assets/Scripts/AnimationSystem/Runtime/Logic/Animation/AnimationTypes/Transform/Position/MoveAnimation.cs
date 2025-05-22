namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Transform.Position
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Transform;
    using Graph.Animations.CreationTools.ParameterTypes;
    using GraphProcessor;
    using Interfaces;
    using ParameterTypes;
    using System;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [Serializable]
    public class MoveAnimation : IAnimable
    {
        // UI Move components
        [Header("UI Move")]
        [SerializeField, Tooltip("Object you want to move on Canvas")]
        private RectTransform objectToMove;

        #region MoveParameters
        
        [Space(10)]
        [Header("Position Move")]
        [SerializeField]
        private bool resetPositionToInitial = false;
        [SerializeField]
        private Vector3ParameterData initialPosition;
        [SerializeField, Tooltip("Object target anchored position at the end of animation")]
        private Vector3ParameterData targetPosition;
        #endregion
        
        // Main animation config
        public SequenceAddType SequenceAddType { get; private set; }

        [field: Space(10)]
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
            var node = BaseNode.CreateFromType<MoveAnimationNode>(position);
            node.MoveAnimation = this;
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

        public Tween GetTween()
        {
            if (resetPositionToInitial)
            {
                objectToMove.anchoredPosition = initialPosition.ParameterValue;
            }
            return objectToMove.DOAnchorPos(targetPosition.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToMove = gameObject.GetComponent<RectTransform>();
        }

        public void SetParameter(Vector3ParameterData value, bool isInitial)
        {
            if (isInitial)
            {
                initialPosition = value;
            }
            else
            {
                targetPosition = value;
            }
        }
    }
}