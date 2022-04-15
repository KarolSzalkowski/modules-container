namespace AnimationSystem.Graph.Animations.AnimationNodes.Transform
{
	using UnityEngine;
	using GraphProcessor;
	using AnimationSystem.Logic.Animation;
	using System;
	using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position;
	using AnimationSystem.Logic.Animation.ParameterTypes;
    using AnimationSystem.Logic.Animation.AnimationTypes.Transform.Scale;
    using AnimationSystem.Graph.Animations.Creation;

    [System.Serializable, NodeMenuItem("Animation/Transform/Size Delta Animation")]
	public class ChangeRectSizeAnimationNode : AnimationNode
	{
		#region Inspector Data
		public ChangeRectSizeAnimation ChangeRectSizeAnimation;
		#endregion
		
		[Input(name = "Target Size")]
		public Vector3 targetSize;

		public override string name => "Change Rect Size Animation";

		protected override void Process()
		{
		}

        public override void SetParameters(ParametersContainer parametersContainer)
        {
			SetParameter(parametersContainer, "targetSize");
		}

        public override SequenceTransitionData GetSequenceData(SequenceAddType sequenceAddType)
		{
			SetAnimableObject((GameObject)GetAssignedParameter().parameter.value);
			var data = new SequenceTransitionData(ChangeRectSizeAnimation.GetTween(), sequenceAddType);
			return GetSequenceDataFromPorts(data);
		}

		public override Type GetNeededType()
		{
			return ChangeRectSizeAnimation.GetAnimableType();
		}

		public override ParameterNode GetAssignedParameter()
		{
			var edge = inputPorts.Find(p => p.fieldName == "animableGo").GetEdges()[0];
			return edge.outputNode as ParameterNode;
		}

		public override void SetAnimableObject(GameObject gameObject)
		{
			ChangeRectSizeAnimation.SetAnimableObject(gameObject);
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
				ChangeRectSizeAnimation.SetTargetSize(GetVector3ParameterWithName(parametersContainer.Vector3ParameterDatas, param.parameter.name));
			}
		}
	}
}