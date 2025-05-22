namespace AnimationSystem.Runtime.ShapesKit
{
    using _3rdParty.ThisOtherThing.UI_Shapes_Kit.Geometry.Shapes;
    using System;
    using System.Collections.Generic;
    
    public static class ShapesMap
    {
        public static Dictionary<ShapeType, Type> ShapesMapDictionary = new()
        {
            { ShapeType.Ellipse, typeof(Ellipse) },
            { ShapeType.Rectangle, typeof(Rectangle) },
            { ShapeType.Polygon, typeof(Polygon) },
            { ShapeType.Line, typeof(Line) },
            { ShapeType.EdgeGradient, typeof(EdgeGradient) }
        };
    }
}
