namespace Utils.UIToolkit
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class StylePropertyHelper
    {
        public static Tween GetStyleTween(IStyle style, IResolvedStyle resolvedStyle, StyleParameterType parameterName, float target, float duration)
        {
            switch (parameterName)
            {
                // position
                case StyleParameterType.Left:
                    return DOTween.To(() => resolvedStyle.left, x => style.left = x, target, duration);
                case StyleParameterType.Right:
                    return DOTween.To(() => resolvedStyle.right, x => style.right = x, target, duration);
                case StyleParameterType.Top:
                    return DOTween.To(() => resolvedStyle.top, x => style.top = x, target, duration);
                case StyleParameterType.Bottom:
                    return DOTween.To(() => resolvedStyle.bottom, x => style.bottom = x, target, duration);

                // size
                case StyleParameterType.Width:
                    return DOTween.To(() => resolvedStyle.width, x => style.width = x, target, duration);
                case StyleParameterType.Height:
                    return DOTween.To(() => resolvedStyle.height, x => style.height = x, target, duration);

                // visibility
                case StyleParameterType.Opacity:
                    return DOTween.To(() => resolvedStyle.opacity, x => style.opacity = x, target, duration);
            }

            Debug.LogError($"No tween found for parameter {parameterName}");
            return null;
        }

        public static Tween GetStyleTween(IStyle style, IResolvedStyle resolvedStyle, StyleParameterType parameterName, Color target, float duration)
        {
            switch (parameterName)
            {
                case StyleParameterType.BackgroundColor:
                    return DOTween.To(() => resolvedStyle.backgroundColor, x => style.backgroundColor = x, target,
                        duration);
            }

            Debug.LogError($"No tween found for parameter {parameterName}");
            return null;
        }
    }
}