namespace Core.Graph.Animations
{
    using Core.ViewManager.Animation;
    using DG.Tweening;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class SequenceTransitionData
    {
        public Tween Tween;
        public SequenceAddType SequenceAddType;
        public List<SequenceTransitionData> JoinedSequences = new List<SequenceTransitionData>();
        public List<SequenceTransitionData> AppendedSequences = new List<SequenceTransitionData>();

        public SequenceTransitionData(Tween tween, SequenceAddType sequenceAddType)
        {
            Tween = tween;
            SequenceAddType = sequenceAddType;
        }
    }
}