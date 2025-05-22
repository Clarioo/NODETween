namespace AnimationSystem.Runtime.Logic.Animation.Interfaces
{
    using System;
    using AnimationSystem.Runtime.Graph.Animations;
    using DG.Tweening;
    using GraphProcessor;
    using UnityEngine;

    public interface IAnimable
    {        
        string AssignedNodeGUID { get; set; }
        float AnimationTime { get; }
        float Delay { get; }
        int Loops { get; }
        Ease Ease { get; }
        SequenceAddType SequenceAddType { get; }
        Tween GetTween();
        Type GetAnimableType();
        AnimationNode CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter);
        /// <summary>
        /// Method used to inject objects to animation from objects on scene. With this solution we can use objects from scene in ScriptableObjects
        /// </summary>
        /// <param name="gameObject">Main game object that will be tweened</param>
        /// 
        void SetAnimableObject(GameObject gameObject);
    }
}