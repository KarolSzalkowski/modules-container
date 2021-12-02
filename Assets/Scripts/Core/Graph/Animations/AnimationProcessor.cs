namespace Core.Graph.Animations
{
    using GraphProcessor;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AnimationProcessor : BaseGraphProcessor
    {
        public BaseNode firstNode;

        public AnimationProcessor(BaseGraph graph) : base(graph) { }

        public override void Run()
        {
            foreach(var node in graph.nodes)
            {
                if (node.GetType() != typeof(AnimationStartScript))
                {
                    node.OnProcess();
                }
            }

            firstNode = graph.nodes.Find(n => n.GetType() == typeof(AnimationStartScript));
            firstNode.OnProcess();
        }

        public override void UpdateComputeOrder()
        {
            
        }
    }
}