namespace AnimationSystem.Graph.Animations.AnimationNodes.Rendering
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
    using AnimationSystem.Logic.Animation.AnimationTypes.Rendering;

    [System.Serializable, NodeMenuItem("Animation/Rendering/Material Float Animation")]
	public class MaterialFloatAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeMaterialFloatAnimation ChangeMaterialFloatAnimation;
		#endregion

		public override string name => "Material Float Animation";

		protected override void Process()
		{
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(ChangeMaterialFloatAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeMaterialFloatAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeMaterialFloatAnimation.SetAnimableObject(gameObject);
		}

		public override void SetOptionalGOs(GameObject[] optionalGOs)
		{
		}
	}
}