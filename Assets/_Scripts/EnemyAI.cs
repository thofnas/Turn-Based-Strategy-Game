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
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestAction = null; 
        
        foreach (BaseAction baseAction in enemyUnit.GetActionsArray())
        {
            // if enemy cannot afford this action;
            if (!enemyUnit.CanSpendActionPointsOnAction(baseAction)) continue;

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestAction = baseAction;
            } else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();

                if (testEnemyAIAction == null || testEnemyAIAction.ActionValue <= bestEnemyAIAction.ActionValue) continue;
                
                bestEnemyAIAction = testEnemyAIAction;
                bestAction = baseAction;
            }
        }

        if (bestEnemyAIAction == null || !enemyUnit.TrySpendActionPoints(bestAction)) return false;
        
        bestAction.TakeAction(bestEnemyAIAction.EnemyGridPosition, onEnemyAIActionComplete);
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
