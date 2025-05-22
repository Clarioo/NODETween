namespace AnimationSystem.Runtime.Graph.Animations.CreationTools.ParameterTypes
{
    using GraphProcessor;

    [System.Serializable]
    public class StringParameterData : BaseParameterData<string, StringParameter>
    {
        public StringParameterData(string name) : base(name) { }
        public StringParameterData(){}
    }
}
