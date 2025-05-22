namespace Utils.UIToolkit
{

    // DON"T DELETE THIS INFORMATION
    // This enum is used in the AnimationSystem to change the style of a UIElement in a UIDocument
    // When you create a new style parameter type, you need to add it to the list styleParameters in the UIDocumentStyleChange class
    public enum StyleParameterType
    {
        None,
        // position
        Left,
        Right,
        Top,
        Bottom,

        // margin
        MarginLeft,
        MarginRight,
        MarginTop,
        MarginBottom,

        // size
        Width,
        Height,
        Scale,

        // visibility
        Opacity,
        BackgroundColor
    }
}