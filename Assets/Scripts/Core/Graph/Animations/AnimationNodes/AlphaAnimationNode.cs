namespace Core.Graph.Animations.AnimationNodes
{
	using UnityEngine;
	using GraphProcessor;
	using Core.ViewManager.Animation;
    using Core.ViewManager.Animation.AnimationTypes.Rendering.ChangeAlpha;
    using System;

    [System.Serializable, NodeMenuItem("Animation/Rendering/Alpha Animation")]
	public class AlphaAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeAlphaAnimation ChangeAlphaAnimation;
		#endregion

		public override string name => "Alpha Animation";

		protected override void Process()
		{
			var animableObj = GetAssignedParameter();
			ChangeAlphaAnimation.SetAnimableObject(animableObj.output as GameObject);
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			var data = new SequenceTransitionData(ChangeAlphaAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

        public override Type GetNeededType()
        {
			return ChangeAlphaAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
			return inputPorts[0].GetEdges()[0].outputNode as ParameterNode;
		}
	}
}