namespace AnimationSystem.Logic.Animation
{
    using AnimationSystem.Graph.Animations;
    using AnimationSystem.Graph.Animations.Creation;
    using AnimationSystem.Logic.Animation;
    using AnimationSystem.Logic.Animation.Interfaces;
    using DG.Tweening;
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Class responsible for creating and playing animations using listed subanimations
    /// </summary>
    [System.Serializable]
    public class AnimationRunner : SerializedMonoBehaviour
    {
        private Sequence sequence;

        [BoxGroup("Graph"), SerializeField]
        protected AnimationGraphCreator animationGraphCreator;

        public ParametersContainer ParametersContainer => animationGraphCreator.ParametersContainer;

        private void Start()
        {
        }

        [BoxGroup("ANIMATION"), Button("Play")]
        public void Play(Action onFinish = null)
        {
            animationGraphCreator.FillParameters();

            var graphProcessor = new AnimationProcessor(animationGraphCreator.SampleGraph);
            graphProcessor.Run();
        }

        public void PlayInstantly() {}

        /// <summary>
        /// Stops animation and kill sequence
        /// </summary>
        /// <param name="playToEnd">If true animation plays to end and then kill sequence</param>
        private void StopSequence(bool playToEnd = false)
        {
            if (sequence == null || !sequence.IsPlaying()) return;
            sequence.Kill(playToEnd);
        }

        /// <summary>
        /// Completes current sequence
        /// </summary>
        public void Complete()
        {
            sequence.Complete();
        }
        
    }
}