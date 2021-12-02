namespace Core.Graph.Animations.Creation
{
    using GraphProcessor;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Animation Graph", menuName = "Graphs/Animation Graph", order = 0)]
    public class AnimationGraph : BaseGraph
    {
        [field: SerializeField]
        public List<GameObject> AnimableObjects { get; private set; }
        public List<AnimationNode> GraphAnimationNodes => GetAnimationNodes();

        public List<AnimationNode> GetAnimationNodes()
        {
            List<AnimationNode> animationNodes = new List<AnimationNode>();
            foreach(var baseNode in nodes)
            {
                if(baseNode.GetType().IsSubclassOf(typeof(AnimationNode)))
                {
                    animationNodes.Add(baseNode as AnimationNode);
                }
            }
            return animationNodes;
        }
    }
}