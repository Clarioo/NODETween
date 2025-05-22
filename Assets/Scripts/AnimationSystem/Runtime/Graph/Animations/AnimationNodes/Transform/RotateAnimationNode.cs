namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
	using System;
	using CreationTools;
	using CreationTools.ParameterTypes;
	using GraphProcessor;
	using Logic.Animation;
	using Logic.Animation.AnimationTypes.Transform.Rotation;
	using Logic.Animation.Interfaces;
	using UnityEngine;

	[Serializable, NodeMenuItem("Animation/Transform/Rotate Animation")]
	public class RotateAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeRotationAnimation ChangeRotationAnimation = new();

		[Input(name = "Target Rotation"), ShowAsDrawer]
		public Vector3 targetEulersRot;
		#endregion

		public override IAnimable Animable => ChangeRotationAnimation;

		public override string name => "Rotate Animation";

		protected override void Process()
		{
		}

		public override void SetParameters(ParametersContainer parametersContainer)
		{
			SetParameter(parametersContainer, "targetEulersRot");
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(ChangeRotationAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeRotationAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeRotationAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }

        private void SetParameter(ParametersContainer parametersContainer, string portName)
        {
	        var inputPort = inputPorts.Find(p => p.fieldName == portName);
	        var rotPort = inputPort.GetEdges();
	        if (rotPort.Count > 0)
	        {
		        var param = rotPort[0].outputNode as ParameterNode;
		        ChangeRotationAnimation.SetTargetRotation(parametersContainer.GetVector3Parameter(param.parameter.name));
	        }
        }
	}
}