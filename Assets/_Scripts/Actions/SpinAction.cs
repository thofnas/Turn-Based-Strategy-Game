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
    
        public override void TakeAction(GridPosition targetGridPosition, Action onSpinComplete)
        {
            if (IsActive) return;
            
            StartCoroutine(SpinRoutine());
            
            ActionStart(onSpinComplete);
        }
        
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) =>
            new EnemyAIAction {
                EnemyGridPosition = gridPosition,
                ActionValue = 0
            };

        protected override List<GridPosition> GetGridPositions(bool _) => new() { Unit.GetGridPosition() };

        public override string GetActionName() => "Spin";

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
        
            ActionComplete();
        }
    }
}
