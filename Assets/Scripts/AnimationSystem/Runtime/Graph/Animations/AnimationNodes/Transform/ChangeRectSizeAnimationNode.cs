namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
	using System;
	using CreationTools;
	using CreationTools.ParameterTypes;
	using GraphProcessor;
	using Logic.Animation;
	using Logic.Animation.AnimationTypes.Transform.Scale;
	using Logic.Animation.Interfaces;
	using UnityEngine;

	[Serializable, NodeMenuItem("Animation/Transform/Size Delta Animation")]
	public class ChangeRectSizeAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeRectSizeAnimation ChangeRectSizeAnimation = new();
		
		[Input(name = "Target Size")]
		public Vector2 targetSize;
		#endregion
		
		public override IAnimable Animable => ChangeRectSizeAnimation;

		public override string name => "Change Rect Size Animation";

		protected override void Process()
		{
		}

        public override void SetParameters(ParametersContainer parametersContainer)
        {
			SetParameter(parametersContainer, "targetSize");
		}

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(ChangeRectSizeAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeRectSizeAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeRectSizeAnimation.SetAnimableObject(gameObject);
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
				ChangeRectSizeAnimation.SetTargetSize(parametersContainer.GetVector2Parameter(param.parameter.name));
			}
		}
	}
}