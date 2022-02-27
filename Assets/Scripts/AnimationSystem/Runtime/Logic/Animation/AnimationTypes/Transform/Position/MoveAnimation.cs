namespace AnimationSystem.Logic.Animation.AnimationTypes.Transform.Position
{
    using AnimationSystem.Graph.Animations.AnimationNodes.Transform;
    using AnimationSystem.Logic.Animation.Interfaces;
    using AnimationSystem.Logic.Animation.ParameterTypes;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    [System.Serializable]
    public class MoveAnimation : IAnimable
    {
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Object you want to move on Canvas")]
        private RectTransform objectToMove;
        
        
        [BoxGroup("UI Move Config"), SerializeField]
        private bool useAnchor = false;
        
        [BoxGroup("UI Move Config"), SerializeField, Tooltip("Object target anchored position at the end of animation"), HideIf("useAnchor")]
        private Vector2 targetPosition;
        
        [BoxGroup("UI Init Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector2 initialAnchorMin;
        [BoxGroup("UI Init Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector2 initialAnchorMax;

        [BoxGroup("UI Destination Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector2 destinationAnchorMin;
        [BoxGroup("UI Destination Anchor"), SerializeField, ShowIf("useAnchor")]
        private Vector2 destinationAnchorMax;

        
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
                objectToMove.anchoredPosition = Vector2.zero;
                objectToMove.anchorMin = initialAnchorMin;
                objectToMove.anchorMax = initialAnchorMax;
                
                Sequence sequence = DOTween.Sequence();
                sequence.Join(objectToMove.DOAnchorMin(destinationAnchorMin, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops));
                sequence.Join(objectToMove.DOAnchorMax(destinationAnchorMax, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops));
                return sequence;
            }
            else
            {
                return objectToMove.DOAnchorPos(targetPosition, AnimationTime).SetDelay(Delay).SetEase(Ease).SetLoops(Loops);

            }
        }

        public void SetAnimableObject(GameObject gameObject)
        {
            objectToMove = gameObject.GetComponent<RectTransform>();
        }

        public void SetParameter(AnchorType anchorType, Vector2 value)
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

        public void SetParameter(Vector2 value)
        {
            targetPosition = value;
        }
    }
}