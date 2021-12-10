namespace AnimationSystem.Logic.Animation.AnimationTypes.Transform.Layout
{
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [System.Serializable]
    public class LayoutSizeAnimation : IAnimable
    {
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Layout Object you want to scale on Canvas")]
        private LayoutElement objectToScale;
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Object target layoutSize position at the end of animation")]
        private Vector2 targetLayoutSize;

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

        public void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            throw new NotImplementedException();
        }

        public Type GetAnimableType()
        {
            return typeof(LayoutElement);
        }

        public Tween GetTween()
        {
            return objectToScale.DOPreferredSize(targetLayoutSize, AnimationTime).SetDelay(Delay).SetEase(Ease);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToScale = gameObject.GetComponent<LayoutElement>();
        }
    }
}