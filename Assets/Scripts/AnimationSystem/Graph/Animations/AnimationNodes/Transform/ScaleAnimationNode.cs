namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GraphProcessor;
	using System.Linq;
    using DG.Tweening;
    using AnimationSystem.Logic.Animation;
    using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Scale;
    using System;

    [System.Serializable, NodeMenuItem("Animation/Transform/Scale Animation")]
	public class ScaleAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeScaleAnimation ChangeScaleAnimation;
        #endregion

        public override string name => "Scale Animation";

		protected override void Process()
		{
		}

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
        {
			var data = new SequenceTransitionData(ChangeScaleAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data); 
		}

        public override Type GetNeededType()
        {
			return ChangeScaleAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
			return inputPorts[0].GetEdges()[0].outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeScaleAnimation.SetAnimableObject(gameObject);
		}
	}
}