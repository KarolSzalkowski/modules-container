using AnimationSystem.Graph.Animations.Creation.ParameterTypes;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationSystem.Graph.Animations.Creation
{
    [System.Serializable]
    public class ParametersContainer
    {
        public List<Vector3ParameterData> Vector3ParameterDatas;
        public List<FloatParameterData> FloatParameterDatas;
    }
}