namespace TestingScripts
{
    using AnimationSystem.Logic.Animation;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RemoteSettingsTesting : MonoBehaviour
    {
        public Vector3 targetRotation;
        public AnimationRunner targetRunner;

        [Button("Set Rotation")]
        public void SetTargetRotation()
        {
            targetRunner.ParametersContainer.SetParameterValue("RotationValue", targetRotation);
        }
    }
}