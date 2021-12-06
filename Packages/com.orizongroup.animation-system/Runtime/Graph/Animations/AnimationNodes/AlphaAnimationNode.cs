namespace AnimationSystem.Graph.Animations.AnimationNodes
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
    using AnimationSystem.Logic.Animation.AnimationTypes.Rendering.ChangeAlpha;
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

        public override void SetAnimableObject(GameObject gameObject)
        {
			ChangeAlphaAnimation.SetAnimableObject(gameObject);
		}
    }
}