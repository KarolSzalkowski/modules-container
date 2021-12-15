namespace AnimationSystem.Graph.Animations.Creation
{
    using GraphProcessor;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class BaseParameterData<T, U> where U : ExposedParameter
    {
        public string ParameterName;
        public T ParameterValue;
        [HideInInspector]
        public U ExposedParameterType;

        public BaseParameterData(string name)
        {
            ParameterName = name;
        }
    }
}