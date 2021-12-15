using GraphProcessor;

namespace AnimationSystem.Graph.Animations.Creation.ParameterTypes
{
    [System.Serializable]
    public class FloatParameterData : BaseParameterData<float, FloatParameter>
    {
        public FloatParameterData(string name) : base(name) { }
    }
}