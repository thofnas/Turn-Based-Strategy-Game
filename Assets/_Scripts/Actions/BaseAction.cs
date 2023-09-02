using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;
        
        protected Unit Unit;
        protected bool IsActive;
        
        private Action _onActionComplete;

        protected virtual void Awake()
        {
            Unit = GetComponent<Unit>();
        }

        public abstract string GetActionName();

        public abstract void DoAction(GridPosition gridPosition, Action onActionComplete);

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition) => 
            GetValidActionGridPositionList().Contains(gridPosition);

        public abstract List<GridPosition> GetValidActionGridPositionList();
        
        public virtual int GetActionPointsCost() => 1;

        public Unit GetUnit() => Unit;

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            _onActionComplete = onActionComplete;
            
            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void ActionComplete()
        {
            IsActive = false;
            _onActionComplete();
            
            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
