using System;
using Actions;
using Grid;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _timer;
    private State _state;

    private enum State
    {
        WaitingForEnemyTurn = 10,
        TakingTurn = 20,
        Busy = 30
    }

    private void Awake()
    {
        _state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) 
            return;

        switch (_state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                _timer -= Time.deltaTime;
                
                if (_timer > 0f) break;
                
                if (TryTakeEnemyAIAction(SetStateTakingTurn)) 
                    _state = State.Busy;
                else
                    TurnSystem.Instance.NextTurn();
                break;
            case State.Busy:
                break;
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                return true;
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        
        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(actionGridPosition)) return false;

        if (!enemyUnit.TrySpendActionPoints(spinAction)) return false;
        
        spinAction.DoAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }

    private void SetStateTakingTurn()
    {
        _timer = 0.5f;
        _state = State.TakingTurn;
    }
    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        _state = State.TakingTurn;
        _timer = 2f;
    }
}
