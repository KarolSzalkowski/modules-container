namespace Core.Graph.Animations.AnimationNodes
{
	using UnityEngine;
	using GraphProcessor;
	using Core.ViewManager.Animation;
	using System;
    using Core.ViewManager.Animation.AnimationTypes.Transform.Position;

    [System.Serializable, NodeMenuItem("Animation/Transform/Move Animation")]
	public class MoveAnimationNode : AnimationNode
	{
		#region Inspector Data
		public MoveAnimation MoveAnimation;
		#endregion

		public override string name => "Move Animation";

		protected override void Process()
		{
			var animableObj = GetAssignedParameter();
			MoveAnimation.SetAnimableObject(animableObj.output as GameObject);
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			var data = new SequenceTransitionData(MoveAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return MoveAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			return inputPorts[0].GetEdges()[0].outputNode as ParameterNode;
		}
	}
}