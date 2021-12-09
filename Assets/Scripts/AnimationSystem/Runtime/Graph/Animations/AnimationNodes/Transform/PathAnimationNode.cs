namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
    using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position;
    using DG.Tweening;
    using System.Collections.Generic;

    [System.Serializable, NodeMenuItem("Animation/Transform/Path Move Animation")]
	public class PathAnimationNode : AnimationNode
	{
		#region Inspector Data
		public PathAnimation PathAnimation;
		#endregion

		[Output("Look At Animation")]
		public List<SequenceTransitionData> lookAtAnimationNode;

		public override string name => "Path Move Animation";

		protected override void Process()
		{
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			var tween = PathAnimation.GetTween() as Tweener;
			Action lookAt = GetLookAtAnimation();
			if(lookAt != null)
            {
				tween.OnUpdate(new TweenCallback(lookAt));
            }
			var data = new SequenceTransitionData(tween, sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return PathAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			return inputPorts[0].GetEdges()[0].outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			PathAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }

		private Action GetLookAtAnimation()
        {
			var outLookAt = outputPorts.Find(p => p.fieldName == "lookAtAnimationNode");
			if (outLookAt.GetEdges().Count > 0)
            {
				var lookAt = outLookAt.GetEdges()[0].inputNode as LookAtAnimationNode;
				return lookAt.LookAtAnimation.LookAtPosition;
				return null;
            }
			return null;
        }
    }
}