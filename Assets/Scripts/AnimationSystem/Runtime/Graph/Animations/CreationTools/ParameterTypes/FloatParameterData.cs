namespace AnimationSystem.Runtime.Graph.Animations.CreationTools.ParameterTypes
{
    using GraphProcessor;

    [System.Serializable]
    public class FloatParameterData : BaseParameterData<float, FloatParameter>
    {
        public FloatParameterData(string name) : base(name) { }
        public FloatParameterData(){}
    }
}