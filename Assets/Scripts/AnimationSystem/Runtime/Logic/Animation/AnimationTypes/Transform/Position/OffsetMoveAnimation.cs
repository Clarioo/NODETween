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
    public class OffsetMoveAnimation : IAnimable
    {
        // UI Move components
        [Header("UI Move")]
        [SerializeField, Tooltip("Object you want to move on Canvas")]
        private RectTransform objectToMove;

        #region MoveParameters
        [Space(10)]
        [Header("OffsetMove")]
        [SerializeField]
        private bool resetOffsetsToInitial = false;

        [SerializeField]
        private Vector2ParameterData initialOffsetMin;
        [SerializeField]
        private Vector2ParameterData initialOffsetMax;
        
        [SerializeField]
        private Vector2ParameterData destinationOffsetMin;
        [SerializeField]
        private Vector2ParameterData destinationOffsetMax;
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
            var node = BaseNode.CreateFromType<OffsetMoveAnimationNode>(position);
            node.OffsetMoveAnimation = this;
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

            if (resetOffsetsToInitial)
            {
                objectToMove.offsetMin = initialOffsetMin.ParameterValue;
                objectToMove.offsetMax = initialOffsetMax.ParameterValue;
            }
            var offsetMin = objectToMove.offsetMin;
            var offsetMax = objectToMove.offsetMax;
            sequence.Join(DOTween.To(() => offsetMin, x => offsetMin = x, destinationOffsetMin.ParameterValue, AnimationTime));
            sequence.Join(DOTween.To(() => offsetMax, x => offsetMax = x, destinationOffsetMax.ParameterValue, AnimationTime));
            return sequence;
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToMove = gameObject.GetComponent<RectTransform>();
        }

        public void SetParameter(OffsetType offsetType, Vector2ParameterData value)
        {
            switch (offsetType)
            {
                case OffsetType.InitialMin:
                    initialOffsetMin = value;
                    break;
                case OffsetType.InitialMax:
                    initialOffsetMax = value;
                    break;
                case OffsetType.TargetMin:
                    destinationOffsetMin = value;
                    break;
                case OffsetType.TargetMax:
                    destinationOffsetMax = value;
                    break;
            }
        }
    }
}
