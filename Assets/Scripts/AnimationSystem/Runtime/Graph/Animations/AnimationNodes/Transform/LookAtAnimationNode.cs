namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
	using System;
	using GraphProcessor;
	using Logic.Animation;
	using Logic.Animation.AnimationTypes.Transform.Rotation;
	using Logic.Animation.Interfaces;
	using UnityEngine;

	[Serializable, NodeMenuItem("Animation/Transform/Look At Animation")]
	public class LookAtAnimationNode : AnimationNode
	{
		#region Inspector Data
		public LookAtAnimation LookAtAnimation = new();

		[Input("GO To Look At")]
		public GameObject ObjectToLookAt;
		#endregion

		public override IAnimable Animable => LookAtAnimation;
		public override string name => "Look At Animation";

		protected override void Process()
		{
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(LookAtAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return LookAtAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			LookAtAnimation.SetAnimableObject(gameObject);
		}

		public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
			if(optionalGOs.Length > 0)
            {
				LookAtAnimation.SetGOToLookAt(optionalGOs[0]);
            }
        }
    }
}