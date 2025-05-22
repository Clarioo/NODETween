namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.UIToolkit
{
    using System;
    using System.Linq;
    using DG.Tweening;
    using Graph.Animations;
    using GraphProcessor;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Utils.UIToolkit;

    [Serializable]
    public abstract class UIDocumentStyleChange : IAnimable
    {
        protected abstract Type ParameterValueType { get; set; }
        
        [SerializeField]
        protected UIDocument document;

        [SerializeField]
        protected string uiElementName;

        [SerializeField]
        protected StyleParameterType parameterType;

        [field: SerializeField]
        public float Delay { get; private set; }

        [field: SerializeField]
        public int Loops { get; private set; }

        [field: SerializeField]
        public float AnimationTime { get; private set; }

        [field: SerializeField]
        public Ease Ease { get; private set; }

        public SequenceAddType SequenceAddType { get; }
        public string AssignedNodeGUID { get; set; }

        public AnimationNode CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            throw new NotImplementedException();
        }

        public virtual Tween GetTween()
        {
            throw new NotImplementedException();
        }

        public Type GetAnimableType()
        {
            return typeof(UIDocument);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            document = gameObject.GetComponent<UIDocument>();
        }

        public bool IsParameterTypeMatchingValue()
        {
            var isParameterMatched = GetStyleParameters().Any(styleParameter => styleParameter == parameterType);
            if (isParameterMatched) return true;
            Debug.LogError($"Parameter type {parameterType} is not supported for this UIDocument style change");
            parameterType = StyleParameterType.None;

            return false;
        }
        
        private StyleParameterType[] GetStyleParameters()
        {
            return (from typeToStyleParameters in TypeToParameterMap.StyleParametersMap
                where typeToStyleParameters.styleType == ParameterValueType
                select typeToStyleParameters.styleParameters.ToArray()).FirstOrDefault();
        }
    }
}