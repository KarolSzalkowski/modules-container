namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
	using AnimationSystem.Graph.Animations.Creation;
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

		public override void SetParameters(ParametersContainer parametersContainer)
		{
			SetParameter(parametersContainer, "targetEulersRot");
		}

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(ChangeRotationAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeRotationAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeRotationAnimation.SetAnimableObject(gameObject);
		}

        public override void SetOptionalGOs(GameObject[] optionalGOs)
        {
            throw new NotImplementedException();
        }

        private void SetParameter(ParametersContainer parametersContainer, string portName)
        {
	        var inputPort = inputPorts.Find(p => p.fieldName == portName);
	        var rotPort = inputPort.GetEdges();
	        if (rotPort.Count > 0)
	        {
		        var param = rotPort[0].outputNode as ParameterNode;
		        ChangeRotationAnimation.SetTargetRotation(GetVector3ParameterWithName(parametersContainer.Vector3ParameterDatas, param.parameter.name));
	        }
        }
	}
}