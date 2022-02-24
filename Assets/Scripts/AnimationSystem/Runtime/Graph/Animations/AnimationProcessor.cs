namespace AnimationSystem.Graph.Animations
{
    using GraphProcessor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AnimationProcessor : BaseGraphProcessor
    {
        public AnimationProcessor(BaseGraph graph) : base(graph) { }

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

        public override void Run()
        {

        }

        public override void UpdateComputeOrder()
        {
            
        }
    }
}