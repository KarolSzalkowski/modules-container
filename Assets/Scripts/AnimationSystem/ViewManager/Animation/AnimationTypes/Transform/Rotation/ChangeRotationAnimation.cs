namespace AnimationSystem.ViewManager.Animation.AnimationTypes.Transform.Rotation
{
    using AnimationSystem.ViewManager.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class ChangeRotationAnimation : IAnimable
    {
        [BoxGroup("Object Config"), SerializeField, Tooltip("Object you want to change rotation")]
        private RectTransform objectToRotate;
        [BoxGroup("Object Config"), SerializeField, Tooltip("Target Rotation in euler angles")]
        private Vector3 targetEulersRot;

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
            return typeof(RectTransform);
        }

        public Tween GetTween()
        {
            return objectToRotate.DORotate(targetEulersRot, AnimationTime, RotateMode.LocalAxisAdd).SetDelay(Delay).SetEase(Ease);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToRotate = gameObject.GetComponent<RectTransform>();
        }
    }
}