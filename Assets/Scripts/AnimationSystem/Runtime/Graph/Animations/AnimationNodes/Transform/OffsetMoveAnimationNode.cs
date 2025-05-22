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
    
    [Serializable, NodeMenuItem("Animation/Transform/Offset Move Animation")]
    public class OffsetMoveAnimationNode : AnimationNode
    {
        #region Inspector Data
        public OffsetMoveAnimation OffsetMoveAnimation = new();
        
        [Input(name = "Initial Offset Min")]
        public Vector2 initialOffsetMin;

        [Input(name = "Initial Offset Max")]
        public Vector2 initialOffsetMax;

        [Input(name = "Target Offset Min")]
        public Vector2 targetOffsetMin;

        [Input(name = "Target Offset Max")]
        public Vector2 targetOffsetMax;
        
        #endregion
        
        public override IAnimable Animable => OffsetMoveAnimation;

        public override string name => "Offset Move Animation";

        protected override void Process()
        {
        }

        public override void SetParameters(ParametersContainer parametersContainer)
        {
            SetParameter(parametersContainer, "initialOffsetMin", OffsetType.InitialMin);
            SetParameter(parametersContainer, "initialOffsetMax", OffsetType.InitialMax);
            SetParameter(parametersContainer, "targetOffsetMin", OffsetType.TargetMin);
            SetParameter(parametersContainer, "targetOffsetMax", OffsetType.TargetMax);
        }

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
            SetAnimableObject((GameObject)(GetAssignedParameter().parameter.value));
            var data = new SequenceTransitionData(OffsetMoveAnimation.GetTween(), sequenceAddType);
            return GetSequenceDataFromPorts(data);
        }

        public override Type GetNeededType()
        {
            return OffsetMoveAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
            var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
            return edge.outputNode as ParameterNode;
        }

        public override void SetAnimableObject(GameObject gameObject)
        {
            OffsetMoveAnimation.SetAnimableObject(gameObject);
        }

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }
        
        		
        private void SetParameter(ParametersContainer parametersContainer, string portName, OffsetType offsetType)
        {
            var inputPort = inputPorts.Find(p => p.fieldName == portName);
            var rotPort = inputPort.GetEdges();
            if (rotPort.Count > 0)
            {
                var param = rotPort[0].outputNode as ParameterNode;
                OffsetMoveAnimation.SetParameter(offsetType, parametersContainer.GetVector2Parameter(param.parameter.name));
            }
        }
    }
}
