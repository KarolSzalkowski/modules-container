namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
    using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position;

    [System.Serializable, NodeMenuItem("Animation/Transform/Move Animation")]
	public class MoveAnimationNode : AnimationNode
	{
		#region Inspector Data
		public MoveAnimation MoveAnimation;
		#endregion

		public override string name => "Move Animation";

		protected override void Process()
		{
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(MoveAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return MoveAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			MoveAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }
    }
}