namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.TMP
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.CreationTools.ParameterTypes;
    using GraphProcessor;
    using Interfaces;
    using System;
    using TMPro;
    using UnityEngine;
    
    [Serializable]
    public class ChangeFontSpacingAnimation : IAnimable
    {
        // Font color components and parameters
        [Header("Font Color")]
        [SerializeField, Tooltip("TMP to change text color")]
        private TextMeshProUGUI textToManage;
        [SerializeField, Tooltip("TMP font target spacing")]
        private float targetSpacing;

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
            // TODO: Implement this method when node for this animation is created
            return null;
        }

        public Type GetAnimableType()
        {
            return typeof(TextMeshProUGUI);
        }

        public Tween GetTween()
        {
            return DOTween.To(() => textToManage.characterSpacing, x => textToManage.characterSpacing = x, targetSpacing,
                AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            textToManage = gameObject.GetComponent<TextMeshProUGUI>();
        }

        public void SetTargetValue(FloatParameterData value)
        {
            targetSpacing = value.ParameterValue;
        }
    }
}
