namespace AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position
{
    using AnimationSystem.Graph.Animations.AnimationNodes.Transform;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [System.Serializable]
    public class PathAnimation : IAnimable
    {
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Layout Object you want to move with specified path")]
        private Transform objectToMove;
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("List of points that object is going to move trough")]
        private Vector3[] movingPath = new Vector3[1];
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Path movement type")]
        private PathType pathType = PathType.Linear;

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
            var node = BaseNode.CreateFromType<PathAnimationNode>(position);
            node.PathAnimation = this;
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
            return objectToMove.DOLocalPath(movingPath, AnimationTime, pathType).SetDelay(Delay).SetEase(Ease);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToMove = gameObject.GetComponent<Transform>();
        }
    }
}