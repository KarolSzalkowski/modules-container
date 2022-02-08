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
		[Output(name = "First")]
		public int firstNode;

		public override string name => "Animation Start";

		protected override void Process()
		{
			foreach(var outport in outputPorts)
            {
				Debug.Log("OutputPorts: " + outport);
				foreach(var edg in outport.GetEdges())
                {
					Debug.Log("Edge : " + edg);
                }
            }


			var firstPart = outputPorts[0].GetEdges()[0].inputNode as AnimationNode;

			foreach(var edge in graph.edges)
            {
				Debug.Log("Edge found: " + edge);
				if(edge != null)
                {
					Debug.Log("Edge Value: " + edge);
                }
                else
                {
					Debug.Log("Value not found");
                }
            }

			var paramNodes = graph.nodes.FindAll(t => t.GetType() == typeof(ParameterNode));

			foreach(var expParamNode in paramNodes)
            {
				var exp = expParamNode as ParameterNode;
				Debug.Log("Param found: " + exp.parameter.name);
            }

			Debug.Log($"Animation started: {firstPart}");

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