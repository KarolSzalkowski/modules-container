namespace AnimationSystem.ViewManager.Animation.Interfaces
{
    using AnimationSystem.Graph.Animations;
    using DG.Tweening;
    using GraphProcessor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAnimable
    {        
        string AssignedNodeGUID { get; }
        float AnimationTime { get; }
        float Delay { get; }
        Ease Ease { get; }
        SequenceAddType SequenceAddType { get; }
        Tween GetTween();
        Type GetAnimableType();
        void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter);
        void SetAnimableObject(GameObject gameObject);
    }
}