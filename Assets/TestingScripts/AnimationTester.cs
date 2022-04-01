namespace TestingScripts
{
    using AnimationSystem.Logic.Animation;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AnimationTester : MonoBehaviour
    {
        public List<RemoteSettingsTesting> SettingsTestings;

        public void Animate()
        {
            foreach(var runner in SettingsTestings)
            {
                runner.SetTargetRotation();
            }
        }
    }
}