namespace AnimationSystem.ViewManager.Animation
{
    using AnimationSystem.ViewManager.Animation.Interfaces;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable][ExecuteAlways]
    public class AnimableObject
    {
        public string GraphParameterName;
        public GameObject ObjectToAnimate;
        [InlineEditor]
        public ObjectAnimator ObjectAnimator;

        [Button("Generate Animator")]
        public void GenerateAnimator()
        {
            if (ObjectToAnimate != null)
            {
                if(ObjectAnimator == null)
                {
                    if (!ObjectToAnimate.GetComponent<ObjectAnimator>())
                    {
                        ObjectAnimator = ObjectToAnimate.AddComponent<ObjectAnimator>();
                    }
                    else
                    {
                        ObjectAnimator = ObjectToAnimate.GetComponent<ObjectAnimator>();
                    }
                }
            }

        }

        [Button("Sync Objects")]
        public void SetObjectToAnimables()
        {
            if(ObjectAnimator != null)
            {
                foreach (var animable in ObjectAnimator.Animables)
                {
                    CheckIsTypeMatching(animable);
                }
            }

        }

        public void CheckIsTypeMatching(IAnimable animable)
        {
            var animType = animable.GetAnimableType();
            AddComponentWithType(animType);
            animable.SetAnimableObject(ObjectToAnimate);
        }

        public void AddComponentWithType(Type animType)
        {
            var obj = ObjectToAnimate.GetComponent(animType);
            if (obj == null)
            {
                ObjectToAnimate.AddComponent(animType);
            }
        }
    }
}