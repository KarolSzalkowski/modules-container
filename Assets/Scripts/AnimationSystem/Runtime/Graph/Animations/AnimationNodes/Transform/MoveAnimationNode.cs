namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
    using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position;
    using AnimationSystem.Logic.Animation.ParameterTypes;
    using AnimationSystem.Graph.Animations.Creation;

    [System.Serializable, NodeMenuItem("Animation/Transform/Move Animation")]
	public class MoveAnimationNode : AnimationNode
	{
		#region Inspector Data
		public MoveAnimation MoveAnimation;
		#endregion

		[Input(name = "Target Pos")]
		public Vector3 targetPosition;

		[Input(name = "Initial Anch Min")]
		public Vector3 initialAnchMin;

		[Input(name = "Initial Anch Max")]
		public Vector3 initialAnchMax;

		[Input(name = "Target Anch Min")]
		public Vector3 targetAnchMin;

		[Input(name = "Target Anch Max")]
		public Vector3 targetAnchMax;

		public override string name => "Move Animation";

		protected override void Process()
		{
		}

        public override void SetParameters(ParametersContainer parametersContainer)
        {
			SetParameter(parametersContainer, "targetPosition");
			SetParameter(parametersContainer, "initialAnchMin", AnchorType.InitialMin);
			SetParameter(parametersContainer, "initialAnchMax", AnchorType.InitialMax);
			SetParameter(parametersContainer, "targetAnchMin", AnchorType.TargetMin);
			SetParameter(parametersContainer, "targetAnchMax", AnchorType.TargetMax);
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

		private void SetParameter(ParametersContainer parametersContainer, string portName, AnchorType anchorType)
        {
			var inputPort = inputPorts.Find(p => p.fieldName == portName);
			var rotPort = inputPort.GetEdges();
			if (rotPort.Count > 0)
			{
				var param = rotPort[0].outputNode as ParameterNode;
				MoveAnimation.SetParameter(anchorType, GetVector3ParameterWithName(parametersContainer.Vector3ParameterDatas, param.parameter.name));
			}
		}

		private void SetParameter(ParametersContainer parametersContainer, string portName)
        {
			var inputPort = inputPorts.Find(p => p.fieldName == portName);
			var rotPort = inputPort.GetEdges();
			if (rotPort.Count > 0)
			{
				var param = rotPort[0].outputNode as ParameterNode;
				MoveAnimation.SetParameter(GetVector3ParameterWithName(parametersContainer.Vector3ParameterDatas, param.parameter.name));
			}
		}
    }
}