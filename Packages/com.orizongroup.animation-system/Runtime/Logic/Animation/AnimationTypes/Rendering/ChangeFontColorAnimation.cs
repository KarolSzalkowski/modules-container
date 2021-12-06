namespace AnimationSystem.Logic.Animation.AnimationTypes.Rendering.FontColor
{
    using AnimationSystem.Logic.Animation;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using TMPro;
    using UnityEngine;

    [System.Serializable]
    public class ChangeFontColorAnimation : IAnimable
    {
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("TMP to change text color")]
        private TextMeshProUGUI textToManage;
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("TMP font target color")]
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
            return typeof(TextMeshProUGUI);
        }

        public Tween GetTween()
        {
            return textToManage.DOColor(targetColor, AnimationTime).SetDelay(Delay).SetEase(Ease);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            textToManage = gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
}