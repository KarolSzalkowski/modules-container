namespace AnimationSystem.Logic.Animation.AnimationTypes.Transform.Scale
{
    using AnimationSystem.Graph.Animations.AnimationNodes.Transform;
    using AnimationSystem.Graph.Animations.Creation.ParameterTypes;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class ChangeRectSizeAnimation : IAnimable
    {
        [BoxGroup("Object Config"), SerializeField, Tooltip("Object you want to change size")]
        private RectTransform objectToScale;
        [BoxGroup("Object Config"), SerializeField, Tooltip("Target Size")]
        private Vector3ParameterData targetSize;

        [field: SerializeField, BoxGroup("Main animation config")]
        public SequenceAddType SequenceAddType { get; private set; }

        [field: SerializeField, BoxGroup("Main animation config")]
        public float Delay { get; private set; }

        [field: SerializeField, BoxGroup("Main animation config")]
        public int Loops { get; private set; }

        [field: SerializeField, BoxGroup("Main animation config")]
        public float AnimationTime { get; private set; }

        [field: SerializeField, BoxGroup("Main animation config")]
        public Ease Ease { get; private set; }

        [field: SerializeField, HideInInspector]
        public string AssignedNodeGUID { get; private set; }

        public Type GetAnimableType()
        {
            return typeof(RectTransform);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToScale = gameObject.GetComponent<RectTransform>();
        }

        public Tween GetTween()
        {
            return objectToScale.DOSizeDelta(targetSize.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            var node = BaseNode.CreateFromType<ChangeRectSizeAnimationNode>(position);
            node.ChangeRectSizeAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
        }

        public void SetTargetSize(Vector2 size)
        {
            targetSize.ParameterValue = size;
        }
    }
}
