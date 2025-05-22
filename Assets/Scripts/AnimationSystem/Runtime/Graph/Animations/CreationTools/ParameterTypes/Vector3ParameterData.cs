namespace AnimationSystem.Runtime.Graph.Animations.CreationTools.ParameterTypes
{
    using System;
    using GraphProcessor;
    using UnityEngine;

    [Serializable]
    public class Vector3ParameterData : BaseParameterData<Vector3, Vector3Parameter>
    {
        public Vector3ParameterData(string name) : base(name) { }
        public Vector3ParameterData(){}
    }
}
