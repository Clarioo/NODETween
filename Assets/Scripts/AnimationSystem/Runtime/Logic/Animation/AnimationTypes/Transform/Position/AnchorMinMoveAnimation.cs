namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Transform.Position
{
    using DG.Tweening;
    using Graph.Animations;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;

    [Serializable]
    public class AnchorMinMoveAnimation : IAnimable
    {
        // AnchorMin components and parameters
        [Header("AnchorMin")]
        [SerializeField, Tooltip("Object you want to move on Canvas")]
        private RectTransform targetRect;

        [SerializeField, Tooltip("Object target anchored position at the end of animation")]
        private Vector2 targetAnchor;
        
        // Main animation config
        public SequenceAddType SequenceAddType { get; private set; }

        [field: SerializeField]
        public float Delay { get; private set; }

        [field: SerializeField]
        public int Loops { get; private set; }

        [field: SerializeField]
        public float AnimationTime { get; private set; } = .5f;

        [field: SerializeField]
        public Ease Ease { get; private set; } = Ease.OutQuad;

        [field: SerializeField, HideInInspector]
        public string AssignedNodeGUID { get; set; }

        public AnimationNode CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            // TODO: Implement this method when node for this animation is created
            return null;
        }

        public Type GetAnimableType()
        {
            return typeof(RectTransform);
        }

        public Tween GetTween()
        {
            var anchorTween = targetRect.DOAnchorMin(new Vector2(targetAnchor.x, targetAnchor.y), 
                    AnimationTime).SetEase(Ease).SetDelay(Delay).SetLoops(Loops);
                anchorTween.onUpdate += () =>
            {
                targetRect.offsetMax = Vector2.zero;
                targetRect.offsetMin = Vector2.zero;
            };
            return anchorTween;
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            targetRect = gameObject.GetComponent<RectTransform>();
        }
    }
}