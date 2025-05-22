namespace AnimationSystem.Runtime.Graph.Animations.AnimationNodes.Transform
{
	using System;
	using DG.Tweening;
	using GraphProcessor;
	using Logic.Animation;
	using Logic.Animation.AnimationTypes.Transform.Position;
	using Logic.Animation.Interfaces;
	using UnityEngine;

	[Serializable, NodeMenuItem("Animation/Transform/Path Move Animation")]
	public class PathAnimationNode : AnimationNode
	{
		#region Inspector Data
		public PathAnimation PathAnimation = new();

		[Output("Look At Animation")]
		public int lookAtAnimationNode;
		#endregion

		public override IAnimable Animable => PathAnimation;

		public override string name => "Path Move Animation";

		protected override void Process()
		{
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var tween = PathAnimation.GetTween() as Tweener;
			Action lookAt = GetLookAtAnimation();
			if(lookAt != null)
            {
				tween.OnUpdate(new TweenCallback(lookAt));
            }
			var data = new SequenceTransitionData(tween, sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return PathAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			PathAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }

		private Action GetLookAtAnimation()
        {
			var outLookAt = outputPorts.Find(p => p.fieldName == "lookAtAnimationNode");
			if (outLookAt.GetEdges().Count > 0)
            {
				var lookAt = outLookAt.GetEdges()[0].inputNode as LookAtAnimationNode;
				return lookAt.LookAtAnimation.LookAtPosition;
            }
			return null;
        }
    }
}