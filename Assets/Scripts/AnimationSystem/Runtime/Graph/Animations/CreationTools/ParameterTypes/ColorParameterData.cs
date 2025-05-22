namespace AnimationSystem.Runtime.Graph.Animations.CreationTools.ParameterTypes
{
    using System;
    using GraphProcessor;
    using UnityEngine;

    [Serializable]
    public class ColorParameterData : BaseParameterData<Color, ColorParameter>
    {
        public ColorParameterData(string name) : base(name) { }
        public ColorParameterData(){}
    }
}
