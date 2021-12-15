using AnimationSystem.Graph.Animations.Creation.ParameterTypes;
using GraphProcessor;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationSystem.Graph.Animations.Creation
{
    [System.Serializable]
    public class ParametersContainer
    {
        public List<Vector3ParameterData> Vector3ParameterDatas;
        public List<FloatParameterData> FloatParameterDatas;

        public void SetParameterValue<T>(string paramName, T value)
        {
            //switch (typeof(T))
            //{
            //    case Vector3ParameterData:
            //        Vector3ParameterDatas.Find(p => p.ParameterName == paramName).ParameterValue = value;
            //        break;
            //}
        }
    }
}