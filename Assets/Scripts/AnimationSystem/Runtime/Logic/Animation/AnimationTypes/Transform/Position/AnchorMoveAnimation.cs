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

    [Serializable]
    public class AnchorMoveAnimation : IAnimable
    {
        // UI Move components
        [Header("UI Move")]
        [SerializeField, Tooltip("Object you want to move on Canvas")]
        private RectTransform objectToMove;

        #region MoveParameters
        
        [Space(10)]
        [Header("Anchor Move")]
        [SerializeField]
        private bool resetAnchorToInitial = false;
        
        [SerializeField]
        private Vector2ParameterData initialAnchorMin;
        [SerializeField]
        private Vector2ParameterData initialAnchorMax;
        
        [SerializeField]
        private Vector2ParameterData destinationAnchorMin;
        [SerializeField]
        private Vector2ParameterData destinationAnchorMax;
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
            var node = BaseNode.CreateFromType<AnchorMoveAnimationNode>(position);
            node.AnchorMoveAnimation = this;
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
            Sequence sequence = DOTween.Sequence();
            if (resetAnchorToInitial)
            {
                objectToMove.anchoredPosition = Vector2.zero;
                objectToMove.anchorMin = initialAnchorMin.ParameterValue;
                objectToMove.anchorMax = initialAnchorMax.ParameterValue;
            }

            sequence.Join(objectToMove.DOAnchorMin(destinationAnchorMin.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops));
            sequence.Join(objectToMove.DOAnchorMax(destinationAnchorMax.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops));
            return sequence;
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToMove = gameObject.GetComponent<RectTransform>();
        }

        public void SetParameter(AnchorType anchorType, Vector2ParameterData value)
        {
            switch (anchorType)
            {
                case AnchorType.InitialMin:
                    initialAnchorMin = value;
                    break;
                case AnchorType.InitialMax:
                    initialAnchorMax = value;
                    break;
                case AnchorType.TargetMin:
                    destinationAnchorMin = value;
                    break;
                case AnchorType.TargetMax:
                    destinationAnchorMax = value;
                    break;
            }
        }
    }
}
