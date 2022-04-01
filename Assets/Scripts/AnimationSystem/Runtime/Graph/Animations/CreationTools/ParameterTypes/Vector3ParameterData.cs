namespace AnimationSystem.Graph.Animations.Creation.ParameterTypes
{
    using GraphProcessor;
    using UnityEngine;

    [System.Serializable]
    public class Vector3ParameterData : BaseParameterData<Vector3, Vector3Parameter>
    {
        public Vector3ParameterData(string name) : base(name) { }
    }
}
