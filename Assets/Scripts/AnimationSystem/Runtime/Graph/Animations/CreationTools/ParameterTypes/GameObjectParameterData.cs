namespace AnimationSystem.Runtime.Graph.Animations.CreationTools.ParameterTypes
{
    using GraphProcessor;
    using UnityEngine;

    [System.Serializable]
    public class GameObjectParameterData : BaseParameterData<GameObject, GameObjectParameter>
    {
        public GameObjectParameterData(string name) : base(name) { }
    }
}