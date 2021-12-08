namespace AnimationSystem.Logic.Animation.AnimationTypes.Rendering
{
    using AnimationSystem.Graph.Animations.AnimationNodes.Rendering;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class ChangeMaterialFloatAnimation : IAnimable
    {
        [BoxGroup("Object Config"), SerializeField, Tooltip("Mesh Renderer with material with property that You want to change")]
        private MeshRenderer rendererWithMatToChange;
        [BoxGroup("Object Config"), SerializeField, Tooltip("Target property Value")]
        private float targetValue;
        [BoxGroup("Object Config"), SerializeField, Tooltip("Target property Name")]
        private string propertyName;

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
            var node = BaseNode.CreateFromType<MaterialFloatAnimationNode>(position);
            node.ChangeMaterialFloatAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
        }

        public Type GetAnimableType()
        {
            return typeof(MeshRenderer);
        }

        public Tween GetTween()
        {
            var tweenParams = new TweenParams().SetDelay(Delay).SetEase(Ease);
            return rendererWithMatToChange.material.DOFloat(targetValue, propertyName, AnimationTime).SetAs(tweenParams);
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            rendererWithMatToChange = gameObject.GetComponent<MeshRenderer>();
        }
    }
}