namespace AnimationSystem.Logic.Animation.AnimationTypes.Rendering.ChangeColor
{
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [System.Serializable]
    public class ChangeColorAnimation : IAnimable
    {
        [BoxGroup("Change Color Config"), SerializeField]
        private Image imageToChange;
        [BoxGroup("Change Color Config"), SerializeField]
        private Color targetColor;

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

        public void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter)
        {
            throw new NotImplementedException();
        }

        public Type GetAnimableType()
        {
            return typeof(Image);
        }

        public Tween GetTween()
        {
            return imageToChange.DOColor(targetColor, AnimationTime).SetDelay(Delay).SetEase(Ease);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            imageToChange = gameObject.GetComponent<Image>();
        }
    }

}
