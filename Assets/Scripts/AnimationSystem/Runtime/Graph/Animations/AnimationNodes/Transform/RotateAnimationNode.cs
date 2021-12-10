namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
    using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Rotation;

    [System.Serializable, NodeMenuItem("Animation/Transform/Rotate Animation")]
	public class RotateAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeRotationAnimation ChangeRotationAnimation;
		#endregion

		[Input(name = "Target Rotation"), ShowAsDrawer]
		public Vector3 targetEulersRot;

		public override string name => "Rotate Animation";

		protected override void Process()
		{
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			var rotPort = inputPorts.Find(p => p.fieldName == "targetEulersRot").GetEdges();
			if (rotPort.Count > 0)
			{
				var param = rotPort[0].outputNode as ParameterNode;
				ChangeRotationAnimation.SetTargetRotation((Vector3)param.parameter.value);
			}
			var data = new SequenceTransitionData(ChangeRotationAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeRotationAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			return inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0].outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeRotationAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }
    }
}