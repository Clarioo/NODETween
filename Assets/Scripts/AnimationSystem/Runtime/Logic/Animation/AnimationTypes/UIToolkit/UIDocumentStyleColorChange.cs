namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.UIToolkit
{
    using System;
    using DG.Tweening;
    using Graph.Animations.CreationTools.ParameterTypes;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Utils.UIToolkit;

    [Serializable]
    public class UIDocumentStyleColorChange : UIDocumentStyleChange
    {
        protected override Type ParameterValueType { get; set; } = typeof(Color);

        [SerializeField]
        private Color targetValue;

        public override Tween GetTween()
        {
            var root = document.rootVisualElement;
            var element = root.Q<VisualElement>(uiElementName);
            if (element == null)
            {
                Debug.LogError($"Element with name {uiElementName} not found in UIDocument {document.name}");
            }

            var tween = StylePropertyHelper.GetStyleTween(element.style, element.resolvedStyle, parameterType,
                targetValue,
                AnimationTime);
            return tween.SetDelay(Delay).SetLoops(Loops).SetEase(Ease);
        }

        public void SetTargetValue(ColorParameterData value)
        {
            targetValue = value.ParameterValue;
        }
    }
}
