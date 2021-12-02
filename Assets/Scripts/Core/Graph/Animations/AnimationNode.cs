using System.Collections;
namespace Core.Graph.Animations
{
	using System.Collections.Generic;
	using UnityEngine;
	using GraphProcessor;
	using System.Linq;
	using DG.Tweening;
    using Core.ViewManager.Animation;
    using Core.ViewManager.Animation.Interfaces;
    using Core.ViewManager.Animation.AnimationTypes.Rendering.ChangeAlpha;
    using System;

    [System.Serializable, NodeMenuItem("Custom/AnimationNode")]
	public abstract class AnimationNode : BaseNode
	{
		public int NodeId;

		#region InOut
		[Input(name = "In")]
		public List<SequenceTransitionData> input;
		[Input(name = "Animable GO")]
		public GameObject animableGo;

		[Output(name = "Append")]
		public List<SequenceTransitionData> outputAppend;
		[Output(name = "Join")]
		public List<SequenceTransitionData> outputJoin;
		#endregion

		public override string name => "AnimationNode";

		protected override void Process()
		{
			
        }

		public abstract SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType);

		public virtual SequenceTransitionData GetSequenceDataFromPorts(SequenceTransitionData data)
        {
			var appendEdges = outputPorts[1].GetEdges();
			if (appendEdges.Count > 0)
			{
				foreach (var edge in appendEdges)
				{
					var node = edge.inputNode as AnimationNode;
					data.AppendedSequences.Add(node.GetSequenceData(SequenceAddType.Append));
				}
			}
			var joinedEdges = outputPorts[0].GetEdges();
			if (joinedEdges.Count > 0)
			{
				foreach (var edge in joinedEdges)
				{
					var node = edge.inputNode as AnimationNode;
					data.JoinedSequences.Add(node.GetSequenceData(SequenceAddType.Join));
				}
			}
			return data;
		}

		public abstract ParameterNode GetAssignedParameter();

		public abstract Type GetNeededType();
	}
}