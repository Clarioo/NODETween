namespace Utils.UIToolkit
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class TypeToParameterMap
    {
        public struct TypeToStyleParameters
        {
            public Type styleType;
            public List<StyleParameterType> styleParameters;
        }
        
        public static List<TypeToStyleParameters> StyleParametersMap = new()
        {
            new TypeToStyleParameters
            {
                styleType = typeof(float),
                styleParameters = new List<StyleParameterType>()
                {
                    // Position
                    StyleParameterType.Left, StyleParameterType.Right, StyleParameterType.Top, StyleParameterType.Bottom,
                    // Margin
                    StyleParameterType.MarginLeft, StyleParameterType.MarginRight, StyleParameterType.MarginTop,
                    StyleParameterType.MarginBottom,
                    // Size
                    StyleParameterType.Width, StyleParameterType.Height,
                    // Visibility
                    StyleParameterType.Opacity
                }
            },
            new TypeToStyleParameters
            {
                styleType = typeof(Color),
                styleParameters = new List<StyleParameterType>()
                {
                    StyleParameterType.BackgroundColor
                }
            },
            new TypeToStyleParameters
            {
                styleType = typeof(Vector2),
                styleParameters = new List<StyleParameterType>()
                {
                    StyleParameterType.Scale
                }
            }
        };
        
        public static bool ValidateType(Type type, StyleParameterType styleParameterType)
        {
            foreach (var typeToStyleParameters in StyleParametersMap)
            {
                if (typeToStyleParameters.styleType == type)
                {
                    return typeToStyleParameters.styleParameters.Contains(styleParameterType);
                }
            }

            return false;
        }
    }
}
