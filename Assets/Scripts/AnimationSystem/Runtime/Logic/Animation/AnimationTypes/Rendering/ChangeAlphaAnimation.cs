namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Rendering
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Rendering;
    using Graph.Animations.CreationTools.ParameterTypes;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;

    [Serializable]
    public class ChangeAlphaAnimation : IAnimable
    {
        // Alpha components and parameters
        [SerializeField]
        private CanvasGroup canvasToChange;
        [SerializeField]
        private FloatParameterData targetAlpha;

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

        public Type GetAnimableType()
        {
            return typeof(CanvasGroup);
        }

        public AnimationNode CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            var node = BaseNode.CreateFromType<AlphaAnimationNode>(position);
            node.ChangeAlphaAnimation = this;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
            return node;
        }

        public Tween GetTween()
        {
            return canvasToChange.DOFade(targetAlpha.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            canvasToChange = gameObject.GetComponent<CanvasGroup>();
        }

        public void SetTargetAlpha(FloatParameterData target)
        {
            targetAlpha = target;
        }
    }

}