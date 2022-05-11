namespace AnimationSystem.Logic.Animation.AnimationTypes.Rendering.ChangeAlpha
{
    using AnimationSystem.Graph.Animations.AnimationNodes.Rendering;
    using AnimationSystem.Graph.Animations.Creation.ParameterTypes;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class ChangeAlphaAnimation : IAnimable
    {
        [BoxGroup("Change Color Config"), SerializeField]
        private CanvasGroup canvasToChange;
        [BoxGroup("Change Color Config"), SerializeField]
        private FloatParameterData targetAlpha;

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
            return typeof(CanvasGroup);
        }

        public void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            var node = BaseNode.CreateFromType<AlphaAnimationNode>(position);
            node.ChangeAlphaAnimation = this;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
        }

        public Tween GetTween()
        {
            return canvasToChange.DOFade(targetAlpha.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            canvasToChange = gameObject.GetComponent<CanvasGroup>();
        }

        public void SetTargetAlpha(FloatParameterData target)
        {
            targetAlpha = target;
        }
    }

}