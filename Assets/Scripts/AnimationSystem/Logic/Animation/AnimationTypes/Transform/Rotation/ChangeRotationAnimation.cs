namespace AnimationSystem.Logic.Animation.AnimationTypes.Transform.Rotation
{
    using AnimationSystem.Graph.Animations.AnimationNodes.Transform;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class ChangeRotationAnimation : IAnimable
    {
        [BoxGroup("Object Config"), SerializeField, Tooltip("Object you want to change rotation")]
        private Transform objectToRotate;
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
            var node = BaseNode.CreateFromType<RotateAnimationNode>(position);
            node.ChangeRotationAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
        }

        public Type GetAnimableType()
        {
            return typeof(Transform);
        }

        public Tween GetTween()
        {
            return objectToRotate.DOLocalRotate(targetEulersRot, AnimationTime, RotateMode.Fast).SetDelay(Delay).SetEase(Ease);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToRotate = gameObject.GetComponent<Transform>();
        }
    }
}