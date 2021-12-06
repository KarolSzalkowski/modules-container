namespace AnimationSystem.Logic.Animation.AnimationTypes.Transform.Scale
{
    using AnimationSystem.Graph.Animations.AnimationNodes;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class ChangeScaleAnimation : IAnimable
    {
        [BoxGroup("Object Config"), SerializeField, Tooltip("Object you want to change size")]
        private RectTransform objectToScale;
        [BoxGroup("Object Config"), SerializeField, Tooltip("Target Scale")]
        private Vector3 targetScale;

        [field: SerializeField, BoxGroup("Main animation config")]
        public SequenceAddType SequenceAddType { get; private set; }

        [field: SerializeField, BoxGroup("Main animation config")]
        public float Delay { get; private set; }
        
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
            return objectToScale.DOScale(targetScale, AnimationTime).SetDelay(Delay).SetEase(Ease);
        }

        public void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            var node = ScaleAnimationNode.CreateFromType<ScaleAnimationNode>(position);
            node.ChangeScaleAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
        }
    }
}
