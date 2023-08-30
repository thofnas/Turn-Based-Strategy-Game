using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
        private readonly float _spinDegrees = 720f;
    
        public override void DoAction(GridPosition gridPosition, Action onSpinComplete)
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

        public override List<GridPosition> GetValidActionGridPositionList() => new() { Unit.GetGridPosition() };

        public override string GetActionName() => "Spin";
    }
}
