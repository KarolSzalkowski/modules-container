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

        public void SetParameterValue(string parameterName, Vector3 value)
        {
            Vector3ParameterDatas.Find(p => p.ParameterName == parameterName).ParameterValue = value;
        }
        
        public void SetParameterValue(string parameterName, float value)
        {
            FloatParameterDatas.Find(p => p.ParameterName == parameterName).ParameterValue = value;
        }
    }
}