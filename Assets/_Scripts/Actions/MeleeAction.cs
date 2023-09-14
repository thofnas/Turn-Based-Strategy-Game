using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public class MeleeAction : BaseAction
    {
        
        private enum State
        {
            SwingingSwordBeforeHit,
            SwingingSwordAfterHit,
        }

        private const float ROTATE_SPEED = 10f;

        public static event EventHandler OnAnyMeleeHit;
        public event EventHandler OnMeleeActionStarted; 
        public event EventHandler OnMeleeActionCompleted; 

        private readonly float _beforeHitStateTime = 0.5f;
        private readonly float _afterHitStateTime = 1f;
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;

        private void Update()
        {
            if (!IsActive) return;
            
            _stateTimer -= Time.deltaTime;
        
            switch (_state)
            {
                case State.SwingingSwordBeforeHit:
                    Vector3 aimDir = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * ROTATE_SPEED);
                    break;
                case State.SwingingSwordAfterHit:
                    break;
            }

            if (_stateTimer <= 0f) NextState();
        }

        public override void TakeAction(GridPosition targetGridPosition, Action onActionComplete)
        {
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(targetGridPosition);
            _state = State.SwingingSwordBeforeHit;
            _stateTimer = _beforeHitStateTime;
            
            ActionStart(onActionComplete);
            
            OnMeleeActionStarted?.Invoke(this, EventArgs.Empty);
        }
    
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) => new() {
            EnemyGridPosition = gridPosition,
            ActionValue = 200
        };

        public override string GetActionName() => "Melee";

        protected override List<GridPosition> GetGridPositions(bool filterByUnitPresence)
        {
            GridPosition unitGridPosition = Unit.GetGridPosition();
            List<GridPosition> validGridPositionList = new();
            int maxDistance = GetMaxDistance();

            for (int x = -maxDistance; x <= maxDistance; x++)
            {
                for (int z = -maxDistance; z <= maxDistance; z++)
                {
                    GridPosition offsetGridPosition = new(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                        continue;

                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                    if (filterByUnitPresence)
                    {
                        if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                            continue;
                        
                        if (targetUnit.IsEnemy() == Unit.IsEnemy())
                                continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
        
        private void NextState()
        {
            switch (_state)
            {
                case State.SwingingSwordBeforeHit:
                    _state = State.SwingingSwordAfterHit;
                    _stateTimer = _afterHitStateTime;

                    _targetUnit.Damage(100, Unit.transform.position);
                    
                    OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
                    break;
                case State.SwingingSwordAfterHit:
                    OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
                    
                    ActionComplete();
                    break;
            }
        }
    }
}
