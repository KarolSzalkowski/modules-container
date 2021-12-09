namespace AnimationSystem.Graph.Animations
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GraphProcessor;
	using System.Linq;
    using AnimationSystem.Logic.Animation;
    using DG.Tweening;
	using AnimationSystem.Graph.Animations.AnimationNodes;


	[System.Serializable, NodeMenuItem("Animation/Animation Start")]
	public class AnimationStartScript : BaseNode
	{
		private float currentAnimationTime = 0;

		[Output(name = "Out")]
		public List<SequenceTransitionData> output;

		public override string name => "Animation Start";

		protected override void Process()
		{
			currentAnimationTime = 0;

			var firstPart = outputPorts[0].GetEdges()[0].inputNode as AnimationNode;

			var sequenceData = firstPart.GetSequenceData(SequenceAddType.Append);

			Sequence sequence = DOTween.Sequence();
			sequence.Append(sequenceData.Tween);

			AddSequence(sequence, sequenceData, 0);

			sequence.onComplete = () => sequence.Complete();
			sequence.SetAutoKill(false);
		}

		private void AddSequence(Sequence seq, SequenceTransitionData sequenceTransitionData, float insertTime)
        {
			foreach (var appended in sequenceTransitionData.AppendedSequences)
			{
				var time = sequenceTransitionData.Tween.Duration() + sequenceTransitionData.Delay;
				seq.Insert(insertTime + time, appended.Tween);
				Debug.Log($"Animation appended {appended.Tween.GetType()} addaed at time: {insertTime + time} with delay {sequenceTransitionData.Delay}");
				AddSequence(seq, appended, insertTime + time);
			}

			foreach (var joined in sequenceTransitionData.JoinedSequences)
			{
				seq.Insert(insertTime, joined.Tween);
				Debug.Log($"Animation joined {joined.Tween.GetType()} addaed at time: {insertTime}");
				AddSequence(seq, joined, insertTime);
			}

		}
	}
}