using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public class MoveAction : BaseAction
    {
        private const float TOLERANCE = 0.1f;
        private const float MOVE_SPEED = 4f;
        private const float ROTATE_SPEED = 10f;

        public event EventHandler OnUnitStartMoving; 
        public event EventHandler OnUnitStopMoving;

        private Vector3 _targetPosition;

        protected override void Awake()
        {
            base.Awake();
            
            _targetPosition = transform.position;
        }

        private void Update()
        {
            if (!IsActive) return;
        
            if (Vector3.Distance(transform.position, _targetPosition) <= TOLERANCE)
            {
                transform.position = _targetPosition;
                
                OnUnitStopMoving?.Invoke(this, EventArgs.Empty);
                
                ActionComplete();
                
                return;
            }

            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * (Time.deltaTime * MOVE_SPEED);
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * ROTATE_SPEED);
        
            OnUnitStartMoving?.Invoke(this, EventArgs.Empty);
        }
    
        public override void DoAction(GridPosition gridPosition, Action onActionComplete)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            
            ActionStart(onActionComplete);
        }
        
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            int targetCount = Unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
            return new EnemyAIAction { EnemyGridPosition = gridPosition, ActionValue = targetCount * 10 };
        }

        protected override List<GridPosition> GetGridPositions(bool filterByUnitPresence)
        {
            List<GridPosition> validGridPositionList = new();

            GridPosition unitGridPosition = Unit.GetGridPosition();

            for (int x = -GetMaxDistance(); x <= GetMaxDistance(); x++)
            {
                for (int z = -GetMaxDistance(); z <= GetMaxDistance(); z++)
                {
                    GridPosition offsetGridPosition = new(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    // if not inside the grid bounds
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                    if (filterByUnitPresence)
                    {
                        // if same position as where unit is standing
                        if (unitGridPosition == testGridPosition) continue;
                
                        // if gridPosition already occupied by any unit
                        if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                    }
                
                    validGridPositionList.Add(testGridPosition);
                }
            }
        
            return validGridPositionList;
        }
        
        public override string GetActionName() => "Move";
    }
}
