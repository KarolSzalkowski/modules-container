namespace AnimationSystem.Graph.Animations
{
    using AnimationSystem.Graph.Animations.Creation;
    using GraphProcessor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AnimationProcessor : BaseGraphProcessor
    {
        public AnimationProcessor(BaseGraph graph, ParametersContainer parametersContainer) : base(graph) 
        {
            this.parametersContainer = parametersContainer;
        }

        private ParametersContainer parametersContainer;

        public void SetParameters()
        {
            var animationNodes = graph.nodes.FindAll(n => n.GetType().IsSubclassOf(typeof(AnimationNode)));
            foreach (var node in animationNodes)
            {
                var animationNode = node as AnimationNode;
                animationNode.SetParameters(parametersContainer);
            }
        }

        public void RunAnimation(Action onComplete)
        {
            foreach (var node in graph.nodes)
            {
                if (node.GetType() != typeof(AnimationStartScript))
                {
                    node.OnProcess();
                }
            }
            var firstNode = graph.nodes.Find(n => n.GetType() == typeof(AnimationStartScript)) as AnimationStartScript;
            firstNode.OnProcess();
            firstNode.ProcessAnimation(onComplete);
        }

        public override void UpdateComputeOrder()
        {
        }

        public override void Run()
        {
        }
    }
}