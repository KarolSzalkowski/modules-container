namespace Core.Graph.Animations.AnimationNodes
{
	using UnityEngine;
	using GraphProcessor;
	using Core.ViewManager.Animation;
	using System;
    using Core.ViewManager.Animation.AnimationTypes.Transform.Rotation;

    [System.Serializable, NodeMenuItem("Animation/Transform/Rotate Animation")]
	public class RotateAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeRotationAnimation ChangeRotationAnimation;
		#endregion

		public override string name => "Rotate Animation";

		protected override void Process()
		{
			var animableObj = GetAssignedParameter();
			ChangeRotationAnimation.SetAnimableObject(animableObj.output as GameObject);
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			var data = new SequenceTransitionData(ChangeRotationAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeRotationAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			return inputPorts[0].GetEdges()[0].outputNode as ParameterNode;
		}
	}
}