namespace AnimationSystem.Graph.Animations
{
    using AnimationSystem.Logic.Animation;
    using DG.Tweening;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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