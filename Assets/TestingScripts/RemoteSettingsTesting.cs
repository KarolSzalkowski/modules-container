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


        private void Start()
        {
            targetRunner = GetComponent<AnimationRunner>();
        }

        [Button("Set Rotation")]
        public void SetTargetRotation()
        {
            //targetRunner.ParametersContainer.SetParameterValue("RotationValue", new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));
            targetRunner.Play();
        }
    }
}