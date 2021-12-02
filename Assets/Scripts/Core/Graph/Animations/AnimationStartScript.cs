namespace Core.Graph.Animations
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GraphProcessor;
	using System.Linq;
    using Core.ViewManager.Animation;
    using DG.Tweening;
	using Core.Graph.Animations.AnimationNodes;


	[System.Serializable, NodeMenuItem("Animation/Animation Start")]
	public class AnimationStartScript : BaseNode
	{
		[Output(name = "Out")]
		public List<SequenceTransitionData> output;

		public override string name => "Animation Start";

		protected override void Process()
		{
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
			var sequenceTime = seq.fullPosition;

			foreach (var appended in sequenceTransitionData.AppendedSequences)
			{
				var time = sequenceTransitionData.Tween.Duration() + sequenceTransitionData.Tween.Delay();
				seq.Insert(insertTime + time, appended.Tween);
				AddSequence(seq, appended, sequenceTime + time);
			}

			foreach (var joined in sequenceTransitionData.JoinedSequences)
			{
				seq.Insert(insertTime, joined.Tween);
				AddSequence(seq, joined, insertTime);
			}

		}
	}
}