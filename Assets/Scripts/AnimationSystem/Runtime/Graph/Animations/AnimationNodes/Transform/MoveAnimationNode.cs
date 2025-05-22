namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
	using System;
	using CreationTools;
	using CreationTools.ParameterTypes;
	using GraphProcessor;
	using Logic.Animation;
	using Logic.Animation.AnimationTypes.Transform.Position;
	using Logic.Animation.Interfaces;
	using Logic.Animation.ParameterTypes;
	using UnityEngine;

	[Serializable, NodeMenuItem("Animation/Transform/Move Animation")]
	public class MoveAnimationNode : AnimationNode
	{
		#region Inspector Data
		public MoveAnimation MoveAnimation = new();

		[Input(name = "Initial Pos")]
		public Vector3 initialPosition;
		[Input(name = "Target Pos")]
		public Vector3 targetPosition;
		
		#endregion

		public override IAnimable Animable => MoveAnimation;

		public override string name => "Move Animation";

		protected override void Process()
		{
		}

        public override void SetParameters(ParametersContainer parametersContainer)
        {
			SetParameter(parametersContainer, "initialPosition");
			SetParameter(parametersContainer, "targetPosition");
		}

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)(GetAssignedParameter().parameter.value));
			var data = new SequenceTransitionData(MoveAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return MoveAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			MoveAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }

		protected override void SetParameter(ParametersContainer parametersContainer, string portName)
        {
			var inputPort = inputPorts.Find(p => p.fieldName == portName);
			var rotPort = inputPort.GetEdges();
			if (rotPort.Count > 0)
			{
				var param = rotPort[0].outputNode as ParameterNode;
				MoveAnimation.SetParameter(parametersContainer.GetVector3Parameter(param.parameter.name), portName == "initialPosition");
			}
		}
    }
}