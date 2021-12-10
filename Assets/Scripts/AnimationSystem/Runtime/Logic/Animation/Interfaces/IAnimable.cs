namespace AnimationSystem.Logic.Animation.Interfaces
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
        int Loops { get; }
        Ease Ease { get; }
        SequenceAddType SequenceAddType { get; }
        Tween GetTween();
        Type GetAnimableType();
        void CreateNode(BaseGraph baseGraph, Vector2 position, ParameterNode goParameter);
        /// <summary>
        /// Method used to inject objects to animation from objects on scene. With this solution we can use objects from scene in ScriptableObjects
        /// </summary>
        /// <param name="gameObject">Main game object that will be tweened</param>
        /// 
        void SetAnimableObject(GameObject gameObject);
    }
}