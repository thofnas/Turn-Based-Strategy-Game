using System;
using System.Collections.Generic;
using Actions;
using Grid;
using UnityEngine;

public class ShootAction : BaseAction
{
    private const int MAX_SHOOT_DISTANCE = 5;
    private const float ROTATE_SPEED = 10f;

    private State _state;
    private float _stateTimer;
    float _aimingStateTime = 1.0f;
    float _shootingStateTime = 0.1f;
    float _cooloffStateTime = 0.1f;
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
    
    private void Shoot()
    {
        _targetUnit.Damage();
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

    public override void DoAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _canShoot = true;
        _state = State.Aiming;
        _stateTimer = _aimingStateTime;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new();

        GridPosition unitGridPosition = Unit.GetGridPosition();

        for (int x = -MAX_SHOOT_DISTANCE; x <= MAX_SHOOT_DISTANCE; x++)
        {
            for (int z = -MAX_SHOOT_DISTANCE; z <= MAX_SHOOT_DISTANCE; z++)
            {
                GridPosition offsetGridPosition = new(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // if not inside the grid bounds
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) 
                    continue;

                float testDistance = Mathf.Sqrt(x * x + z * z);

                if (Mathf.Floor(testDistance) > MAX_SHOOT_DISTANCE)
                    continue;
                
                // if gridPosition is empty
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) 
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                
                // if both units are in the same team
                if (targetUnit.IsEnemy() == Unit.IsEnemy()) 
                    continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }
    
    public override string GetActionName() => "Shoot";
    
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }
}
