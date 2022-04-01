namespace AnimationSystem.Graph.Animations.Creation.ParameterTypes
{
    using GraphProcessor;
    using UnityEngine;

    [System.Serializable]
    public class StringParameterData : BaseParameterData<string, StringParameter>
    {
        public StringParameterData(string name) : base(name) { }
    }
}
