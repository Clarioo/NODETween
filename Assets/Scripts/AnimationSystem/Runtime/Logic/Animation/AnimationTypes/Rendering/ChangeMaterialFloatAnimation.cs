namespace AnimationSystem.Runtime.Logic.Animation.AnimationTypes.Rendering
{
    using DG.Tweening;
    using Graph.Animations;
    using Graph.Animations.AnimationNodes.Rendering;
    using Graph.Animations.CreationTools.ParameterTypes;
    using GraphProcessor;
    using Interfaces;
    using System;
    using UnityEngine;

    [Serializable]
    public class ChangeMaterialFloatAnimation : IAnimable
    {
        // Material components and parameters
        [Header("Material")]
        [SerializeField, Tooltip("Mesh Renderer with material with property that You want to change")]
        private MeshRenderer rendererWithMatToChange;
        [SerializeField, Tooltip("Target property Value")]
        private FloatParameterData targetValue;
        [SerializeField, Tooltip("Target property Name")]
        private string propertyName;

        public SequenceAddType SequenceAddType { get; private set; }

        [field: SerializeField]
        public float Delay { get; private set; }

        [field: SerializeField]
        public int Loops { get; private set; }

        [field: SerializeField]
        public float AnimationTime { get; private set; }

        [field: SerializeField]
        public Ease Ease { get; private set; }

        [field: SerializeField, HideInInspector]
        public string AssignedNodeGUID { get; set; }

        public AnimationNode CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            var node = BaseNode.CreateFromType<MaterialFloatAnimationNode>(position);
            node.ChangeMaterialFloatAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
            return node;
        }

        public Type GetAnimableType()
        {
            return typeof(MeshRenderer);
        }

        public Tween GetTween()
        {
            var tweenParams = new TweenParams().SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
            return rendererWithMatToChange.material.DOFloat(targetValue.ParameterValue, propertyName, AnimationTime).SetAs(tweenParams);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            rendererWithMatToChange = gameObject.GetComponent<MeshRenderer>();
        }
        
        public void SetTargetValue(FloatParameterData value)
        {
            targetValue = value;
        }
    }
}