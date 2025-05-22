namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
    using CreationTools;
    using GraphProcessor;
    using Logic.Animation;
    using Logic.Animation.AnimationTypes.Transform.Position;
    using Logic.Animation.Interfaces;
    using Logic.Animation.ParameterTypes;
    using System;
    using UnityEngine;

    [Serializable, NodeMenuItem("Animation/Transform/Anchor Move Animation")]
    public class AnchorMoveAnimationNode : AnimationNode
    {
        #region Inspector Data
        public AnchorMoveAnimation AnchorMoveAnimation = new();
        
        [Input(name = "Initial Anchor Min")]
        public Vector2 initialAnchMin;

        [Input(name = "Initial Anchor Max")]
        public Vector2 initialAnchMax;

        [Input(name = "Target Anchor Min")]
        public Vector2 targetAnchMin;

        [Input(name = "Target Anchor Max")]
        public Vector2 targetAnchMax;
        
        #endregion
        
        public override IAnimable Animable => AnchorMoveAnimation;

        public override string name => "Anchor Move Animation";

        protected override void Process()
        {
        }

        public override void SetParameters(ParametersContainer parametersContainer)
        {
            SetParameter(parametersContainer, "initialAnchMin", AnchorType.InitialMin);
            SetParameter(parametersContainer, "initialAnchMax", AnchorType.InitialMax);
            SetParameter(parametersContainer, "targetAnchMin", AnchorType.TargetMin);
            SetParameter(parametersContainer, "targetAnchMax", AnchorType.TargetMax);
        }

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
            SetAnimableObject((GameObject)(GetAssignedParameter().parameter.value));
            var data = new SequenceTransitionData(AnchorMoveAnimation.GetTween(), sequenceAddType);
            return GetSequenceDataFromPorts(data);
        }

        public override Type GetNeededType()
        {
            return AnchorMoveAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
            var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
            return edge.outputNode as ParameterNode;
        }

        public override void SetAnimableObject(GameObject gameObject)
        {
            AnchorMoveAnimation.SetAnimableObject(gameObject);
        }

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }
        
        private void SetParameter(ParametersContainer parametersContainer, string portName, AnchorType anchorType)
        {
            var inputPort = inputPorts.Find(p => p.fieldName == portName);
            var rotPort = inputPort.GetEdges();
            if (rotPort.Count > 0)
            {
                var param = rotPort[0].outputNode as ParameterNode;
                AnchorMoveAnimation.SetParameter(anchorType, parametersContainer.GetVector2Parameter(param.parameter.name));
            }
        }
    }
}
