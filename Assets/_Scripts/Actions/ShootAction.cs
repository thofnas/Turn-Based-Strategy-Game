using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public class ShootAction : BaseAction
    {
        private const float ROTATE_SPEED = 10f;

        public event EventHandler<OnUnitShootEventArgs> OnUnitShoot;
    
        public class OnUnitShootEventArgs : EventArgs
        {
            public Unit ShootingUnit;
            public Unit TargetUnit;
        }
    
        private readonly float _aimingStateTime = 1.0f;
        private readonly float _shootingStateTime = 0.1f;
        private readonly float _cooloffStateTime = 0.5f;
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canShoot;

        private void Update()
        {
            if (!IsActive) return;
        
            _stateTimer -= Time.deltaTime;
        
            switch (_state)
            {
                case State.Aiming:
                    Vector3 aimDir = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * ROTATE_SPEED);
                    break;
                case State.Shooting:
                    if (!_canShoot) break;
                    _canShoot = false;
                    Shoot();
                    break;
                case State.Cooloff:
                    break;
            }

            if (_stateTimer <= 0f) NextState();
        }

        public override void DoAction(GridPosition gridPosition, Action onActionComplete)
        {
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            _canShoot = true;
            _state = State.Aiming;
            _stateTimer = _aimingStateTime;
     
            ActionStart(onActionComplete);
        }
        
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            return new EnemyAIAction { 
                EnemyGridPosition = gridPosition,
                ActionValue = 100 + (int)((1 - targetUnit.GetHealthNormalized()) * 100f)
            };
        }

        public override string GetActionName() => "Shoot";

        protected override List<GridPosition> GetGridPositions(bool filterByUnitPresence) =>
            GetGridPositions(filterByUnitPresence, Unit.GetGridPosition());

        public Unit GetTargetUnit() => _targetUnit;

        public int GetTargetCountAtPosition(GridPosition gridPosition) => 
            GetGridPositions(true, gridPosition).Count;

        private List<GridPosition> GetGridPositions(bool filterByUnitPresence, GridPosition gridPosition)
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

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                        continue;

                    float testDistance = Mathf.Sqrt(x * x + z * z);

                    if (Mathf.Round(testDistance) > maxDistance)
                        continue;

                    if (filterByUnitPresence)
                    {
                        if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                            continue;

                        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                        if (targetUnit.IsEnemy() == Unit.IsEnemy())
                            continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
    
        private void Shoot()
        {
            _targetUnit.Damage(40, Unit.transform.position);
        
            OnUnitShoot?.Invoke(this, new OnUnitShootEventArgs {
                ShootingUnit = Unit,
                TargetUnit = _targetUnit
            });
        }
    
        private void NextState()
        {
            switch (_state)
            {
                case State.Aiming:
                    _state = State.Shooting;
                    _stateTimer = _shootingStateTime;
                    break;
                case State.Shooting:
                    _state = State.Cooloff;
                    _stateTimer = _cooloffStateTime;
                    break;
                case State.Cooloff:
                    ActionComplete();
                    break;
            }
        }
    
        private enum State
        {
            Aiming,
            Shooting,
            Cooloff
        }
    }
}
