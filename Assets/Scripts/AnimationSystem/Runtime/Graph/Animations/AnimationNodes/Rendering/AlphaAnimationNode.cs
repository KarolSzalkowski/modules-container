namespace AnimationSystem.Graph.Animations.AnimationNodes.Rendering
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

		[Input(name = "Target Alpha"), ShowAsDrawer]
		public float targetAlpha;

		public override string name => "Alpha Animation";

		protected override void Process()
		{
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			var alphaPort = inputPorts.Find(p => p.fieldName == "targetAlpha").GetEdges();
			if (alphaPort.Count > 0)
            {
				var param = alphaPort[0].outputNode as ParameterNode;
				ChangeAlphaAnimation.SetTargetAlpha((float)param.parameter.value);
            }
			var data = new SequenceTransitionData(ChangeAlphaAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

        public override Type GetNeededType()
        {
			return ChangeAlphaAnimation.GetAnimableType();
        }

        public override ParameterNode GetAssignedParameter()
        {
			return inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0].outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
        {
			ChangeAlphaAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
        }
    }
}