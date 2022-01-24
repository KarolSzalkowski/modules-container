namespace AnimationSystem.Graph.Animations.Creation.ParameterTypes
{
    using GraphProcessor;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class GameObjectParameterData : BaseParameterData<GameObject, GameObjectParameter>
    {
        public GameObjectParameterData(string name) : base(name) { }
    }
}