namespace AnimationSystem.Runtime.Graph.Animations.CreationTools.ParameterTypes
{
    using System;
    using GraphProcessor;
    using UnityEngine;

    [Serializable]
    public class Vector2ParameterData : BaseParameterData<Vector2, Vector2Parameter>
    {
        public Vector2ParameterData(string name) : base(name) { }
        public Vector2ParameterData(){}
    }
}
