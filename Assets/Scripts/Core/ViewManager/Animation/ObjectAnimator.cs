namespace Core.ViewManager.Animation
{
    using Core.ViewManager.Animation.Interfaces;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectAnimator : SerializedMonoBehaviour
    {
        [InlineEditor]
        public List<IAnimable> Animables = new List<IAnimable>();
    }
}