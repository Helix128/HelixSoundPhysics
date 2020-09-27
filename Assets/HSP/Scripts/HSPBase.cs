using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Helix.Audio {
    public class HSPBase : MonoBehaviour
    {
        float stepTimer;
        public void FixedUpdate()
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer < 0)
            {
                stepTimer = HSPConfig.audioStep;
                OnHSPUpdate();
            }
        }
        public virtual void OnHSPUpdate()
        {
         
        }
    }
}