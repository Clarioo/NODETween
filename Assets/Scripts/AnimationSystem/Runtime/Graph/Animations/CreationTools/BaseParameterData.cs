namespace AnimationSystem.Runtime.Graph.Animations.CreationTools
{
    using GraphProcessor;
    using UnityEngine;

    [System.Serializable]
    public class BaseParameterData<T, U> where U : ExposedParameter, new()
    {
        public string ParameterName;
        public T ParameterValue;
        [HideInInspector]
        public U ExposedParameterType;

        public BaseParameterData()
        {
            
        }

        public BaseParameterData(string name)
        {
            ParameterName = name;
        }
    }
}