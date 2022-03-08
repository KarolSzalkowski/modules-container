namespace AnimationSystem.Graph.Animations
{
	using System.Collections.Generic;
	using UnityEngine;
	using GraphProcessor;
	using System.Linq;
	using DG.Tweening;
    using AnimationSystem.Logic.Animation;
    using AnimationSystem.Logic.Animation.Interfaces;
    using AnimationSystem.Logic.Animation.AnimationTypes.Rendering.ChangeAlpha;
    using System;
    using AnimationSystem.Graph.Animations.Creation;
    using AnimationSystem.Graph.Animations.Creation.ParameterTypes;

    [System.Serializable]
	public abstract class AnimationNode : BaseNode
	{
		public int NodeId;

		#region InOut
		[Input(name = "Input")]
		public int animationInput;
		[Input(name = "AnimableGO")]
		public GameObject animableGo;

		[Output(name = "Append")]
		public int outputAppend;
		[Output(name = "Join")]
		public int outputJoin;
		#endregion

		public override string name => "AnimationNode";

		protected override void Process()
		{
			
        }

		public virtual void SetParameters(ParametersContainer parametersContainer)
        {

        }

		public abstract SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType);

		public virtual SequenceTransitionData GetSequenceDataFromPorts(SequenceTransitionData data)
        {
			var appendEdges = outputPorts.Find(p => p.fieldName == "outputAppend").GetEdges();
			if (appendEdges.Count > 0)
			{
				foreach (var edge in appendEdges)
				{
					var node = edge.inputNode as AnimationNode;
					data.AppendedSequences.Add(node.GetSequenceData(SequenceAddType.Append));
				}
			}
			var joinedEdges = outputPorts.Find(p => p.fieldName == "outputJoin").GetEdges();
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

		public abstract void SetAnimableObject(GameObject gameObject);
		public abstract void SetOptionalGOs(GameObject[] optionalGOs);

		public abstract Type GetNeededType();

		protected Vector3ParameterData GetVector3ParameterWithName(List<Vector3ParameterData> parametersContainer, string parameterName)
        {
			return parametersContainer.Find(p => p.ParameterName == parameterName);
        }
	}
}