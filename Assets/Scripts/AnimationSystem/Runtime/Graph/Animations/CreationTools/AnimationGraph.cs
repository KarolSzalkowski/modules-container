namespace AnimationSystem.Graph.Animations.Creation
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

        public List<T> GetNodesOfType<T>() where T : BaseNode
        {
            List<T> typeNodes = new List<T>();
            foreach (var baseNode in nodes)
            {
                if (baseNode.GetType() == typeof(T) )
                {
                    typeNodes.Add(baseNode as T);
                }
            }
            return typeNodes;
        }

        public List<ParameterNode> GetParameterNodesOfType<T>()
        {
            List<ParameterNode> typeNodes = new List<ParameterNode>();
            var paramNodes = GetNodesOfType<ParameterNode>();
            foreach (var paramNode in paramNodes)
            {
                if (paramNode.parameter.value.GetType() == typeof(T))
                {
                    typeNodes.Add(paramNode);
                }
            }
            return typeNodes;
        }

        public List<U> GetParametersOfType<T,U>() where U : ExposedParameter
        {
            List<U> list = new List<U>();
            foreach(var val in exposedParameters)
            {
                if (val.value.GetType() == typeof(T))
                {
                    list.Add(val as U);
                }
            }
            return list;
        }
    }
}