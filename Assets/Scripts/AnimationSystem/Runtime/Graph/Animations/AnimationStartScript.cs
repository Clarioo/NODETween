namespace AnimationSystem.Runtime.Graph.Animations
{
	using System;
	using AnimationSystem.Runtime.Logic.Animation;
	using DG.Tweening;
	using GraphProcessor;
	using UnityEngine;

	[System.Serializable, NodeMenuItem("Animation/Animation Start")]
	public class AnimationStartScript : BaseNode
	{
		[Output(name = "First")]
		public int firstNode;

		public override string name => "Animation Start";

		public Sequence GetSequence()
		{
			var firstPart = outputPorts[0].GetEdges()[0].inputNode as AnimationNode;
			
			var sequenceData = firstPart.GetSequenceData(SequenceAddType.Append);

			Sequence sequence = DOTween.Sequence().Pause().SetAutoKill(false);
			sequence.Append(sequenceData.Tween);

			AddSequence(sequence, sequenceData, 0);
			
			return sequence;
		}

		protected override void Process()
		{

		}

		private void AddSequence(Sequence seq, SequenceTransitionData sequenceTransitionData, float insertTime)
        {
			foreach (var appended in sequenceTransitionData.AppendedSequences)
			{
				var time = sequenceTransitionData.Tween.Duration() + sequenceTransitionData.Delay;
				seq.Insert(insertTime + time, appended.Tween);
				AddSequence(seq, appended, insertTime + time);
			}

			foreach (var joined in sequenceTransitionData.JoinedSequences)
			{
				seq.Insert(insertTime, joined.Tween);
				AddSequence(seq, joined, insertTime);
			}

		}
	}
}