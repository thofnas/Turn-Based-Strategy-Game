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

        private readonly List<Vector3> _targetPositionList = new();
        private int _currentPositionIndex;

        private void Update()
        {
            if (!IsActive) return;

            Vector3 targetPosition = _targetPositionList[_currentPositionIndex];
            
            if (Vector3.Distance(transform.position, targetPosition) > TOLERANCE)
            {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                transform.position += moveDirection * (Time.deltaTime * MOVE_SPEED);
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * ROTATE_SPEED);
            } else
            {
                transform.position = targetPosition;

                _currentPositionIndex++;

                if (_currentPositionIndex < _targetPositionList.Count) return;
                
                OnUnitStopMoving?.Invoke(this, EventArgs.Empty);
                
                ActionComplete();
                
                _targetPositionList.Clear();
            }
        }
    
        public override void DoAction(GridPosition targetGridPosition, Action onActionComplete)
        {
            List<GridPosition> pathGridPositions = 
                Pathfinding.Instance.FindPath(Unit.GetGridPosition(), targetGridPosition, out int pathLength);

            _currentPositionIndex = 0;
            // _targetPositionList = new List<Vector3> {
            //     LevelGrid.Instance.GetWorldPosition(gridPosition)
            // };
            foreach (GridPosition gridPosition in pathGridPositions)
            {
                _targetPositionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
            }
            
            OnUnitStartMoving?.Invoke(this, EventArgs.Empty);

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
            int maxDistance = GetMaxDistance();

            for (int x = -maxDistance; x <= maxDistance; x++)
            {
                for (int z = -maxDistance; z <= maxDistance; z++)
                {
                    GridPosition offsetGridPosition = new(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    // if not inside the grid bounds
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) 
                        continue;

                    if (filterByUnitPresence)
                    {
                        // if same position as where unit is standing
                        if (unitGridPosition == testGridPosition) 
                            continue;
                
                        // if gridPosition already occupied by any unit
                        if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) 
                            continue;
                    }

                    if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                        continue;
                    
                    // if doesn't have path to that grid position
                    if (Pathfinding.Instance.FindPath(unitGridPosition, testGridPosition, out int pathLength) == null)
                        continue;
                    
                    // path way is too long
                    if (pathLength > maxDistance)
                        continue;
                    
                    validGridPositionList.Add(testGridPosition);
                }
            }
        
            return validGridPositionList;
        }
        
        public override string GetActionName() => "Move";
    }
}
