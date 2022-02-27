namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
    using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position;
    using AnimationSystem.Logic.Animation.ParameterTypes;

    [System.Serializable, NodeMenuItem("Animation/Transform/Move Animation")]
	public class MoveAnimationNode : AnimationNode
	{
		#region Inspector Data
		public MoveAnimation MoveAnimation;
		#endregion

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

		public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetParameter("initialAnchMin", AnchorType.InitialMin);
			SetParameter("initialAnchMax", AnchorType.InitialMax);
			SetParameter("targetAnchMin", AnchorType.TargetMin);
			SetParameter("targetAnchMax", AnchorType.TargetMax);

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

		private void SetParameter(string portName, AnchorType anchorType)
        {
			var inputPort = inputPorts.Find(p => p.fieldName == portName);
			var rotPort = inputPort.GetEdges();
			if (rotPort.Count > 0)
			{
				var param = rotPort[0].outputNode as ParameterNode;
				MoveAnimation.SetParameter(anchorType, (Vector3)param.parameter.value);
			}
		}
    }
}