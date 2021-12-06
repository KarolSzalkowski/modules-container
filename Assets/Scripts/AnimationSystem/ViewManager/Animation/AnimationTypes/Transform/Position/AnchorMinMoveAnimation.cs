namespace AnimationSystem.ViewManager.Animation.AnimationTypes.Transform.Position
{
    using System;
    using DG.Tweening;
    using GraphProcessor;
    using Interfaces;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class AnchorMinMoveAnimation : IAnimable
    {
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Object you want to move on Canvas")]
        private RectTransform targetRect;

        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Object target anchored position at the end of animation")]
        private Vector2 targetAnchor;
        
        [field: SerializeField, BoxGroup("Main animation config")]
        public SequenceAddType SequenceAddType { get; private set; }

        [field: SerializeField, BoxGroup("Main animation config")]
        public float Delay { get; private set; }

        [field: SerializeField, BoxGroup("Main animation config")]
        public float AnimationTime { get; private set; } = .5f;

        [field: SerializeField, BoxGroup("Main animation config")]
        public Ease Ease { get; private set; } = Ease.OutQuad;

        [field: SerializeField, HideInInspector]
        public string AssignedNodeGUID { get; private set; }

        public void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            throw new NotImplementedException();
        }

        public Type GetAnimableType()
        {
            return typeof(RectTransform);
        }

        public Tween GetTween()
        {
            var anchorTween = targetRect.DOAnchorMin(new Vector2(targetAnchor.x, targetAnchor.y), 
                    AnimationTime).SetEase(Ease).SetDelay(Delay);
                anchorTween.onUpdate += () =>
            {
                targetRect.offsetMax = Vector2.zero;
                targetRect.offsetMin = Vector2.zero;
            };
            return anchorTween;
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            targetRect = gameObject.GetComponent<RectTransform>();
        }
    }
}