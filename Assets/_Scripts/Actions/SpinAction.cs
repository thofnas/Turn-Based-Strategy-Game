using System;
using System.Collections;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
        private readonly float _spinDegrees = 720f;
    
        public void Spin(Action onSpinComplete)
        {
            if (IsActive) return;

            OnActionComplete = onSpinComplete;
            
            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            IsActive = true;
        
            float degrees = 0f;

            while (degrees < _spinDegrees)
            {
                float speed = _spinDegrees * Time.deltaTime;
                degrees += speed;
                transform.eulerAngles += new Vector3(0f, speed, 0f);
            
                yield return null;
            }
        
            IsActive = false;

            OnActionComplete();
        }
    }
}
