namespace AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position
{
    using AnimationSystem.Graph.Animations.AnimationNodes.Transform;
    using AnimationSystem.Graph.Animations.Creation.ParameterTypes;
    using AnimationSystem.Logic.Animation.Interfaces;
    using AnimationSystem.Logic.Animation.ParameterTypes;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class MoveAnimation : IAnimable
    {
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Object you want to move on Canvas")]
        private RectTransform objectToMove;
        
        
        [BoxGroup("UI Move Config"), SerializeField]
        private bool useAnchor = false;
        
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Object target anchored position at the end of animation"), HideIf("useAnchor")]
        private Vector3ParameterData targetPosition;
        
        [BoxGroup("UI Init Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector3ParameterData initialAnchorMin;
        [BoxGroup("UI Init Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector3ParameterData initialAnchorMax;

        [BoxGroup("UI Destination Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector3ParameterData destinationAnchorMin;
        [BoxGroup("UI Destination Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector3ParameterData destinationAnchorMax;

        
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
            var node = BaseNode.CreateFromType<MoveAnimationNode>(position);
            node.MoveAnimation = this;
            node.expanded = true;
            baseGraph.AddNode(node);
            baseGraph.Connect(node.inputPorts[0], goParameter.outputPorts[0]);
            AssignedNodeGUID = node.GUID;
            baseGraph.NotifyNodeChanged(node);
        }

        public Type GetAnimableType()
        {
            return typeof(RectTransform);
        }

        public Tween GetTween()
        {
            if (useAnchor)
            {
                //Vector3 initAnchMin = InitMinParameter == null ? initialAnchorMin :  (Vector3)InitMinParameter.value;
                //Vector3 initAnchMax = InitMaxParameter == null ? initialAnchorMax : (Vector3)InitMaxParameter.value;
                //Vector3 destAnchMin = DestMinParameter == null ? destinationAnchorMin : (Vector3)DestMinParameter.value;
                //Vector3 destAnchMax = DestMaxParameter == null ? destinationAnchorMax : (Vector3)DestMaxParameter.value;

                objectToMove.anchoredPosition = Vector2.zero;
                objectToMove.anchorMin = initialAnchorMin.ParameterValue;
                objectToMove.anchorMax = initialAnchorMax.ParameterValue;
                
                Sequence sequence = DOTween.Sequence();
                sequence.Join(objectToMove.DOAnchorMin(destinationAnchorMin.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops));
                sequence.Join(objectToMove.DOAnchorMax(destinationAnchorMax.ParameterValue, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops));
                return sequence;
            }
            else
            {
                Vector2 targetPos = targetPosition == null ? targetPosition.ParameterValue : targetPosition.ParameterValue;
                return objectToMove.DOAnchorPos(targetPos, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);
            }
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToMove = gameObject.GetComponent<RectTransform>();
        }

        public void SetParameter(AnchorType anchorType, Vector3ParameterData value)
        {
            switch (anchorType)
            {
                case AnchorType.InitialMin:
                    initialAnchorMin = value;
                    break;
                case AnchorType.InitialMax:
                    initialAnchorMax = value;
                    break;
                case AnchorType.TargetMin:
                    destinationAnchorMin = value;
                    break;
                case AnchorType.TargetMax:
                    destinationAnchorMax = value;
                    break;
            }
        }

        public void SetParameter(Vector3ParameterData value)
        {
            targetPosition = value;
        }
    }
}