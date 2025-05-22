namespace AnimationSystem.Runtime.Graph.Animations
{
    using System.Collections.Generic;
    using AnimationSystem.Runtime.Logic.Animation;
    using DG.Tweening;

    public class SequenceTransitionData
    {
        public Tween Tween;
        public float Delay;
        public SequenceAddType SequenceAddType;
        public List<SequenceTransitionData> JoinedSequences = new List<SequenceTransitionData>();
        public List<SequenceTransitionData> AppendedSequences = new List<SequenceTransitionData>();

        public SequenceTransitionData(Tween tween, SequenceAddType sequenceAddType)
        {
            Tween = tween;
            SequenceAddType = sequenceAddType;
            Delay = Tween.Delay();
        }
    }
}