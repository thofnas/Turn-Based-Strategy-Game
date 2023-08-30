using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Action OnActionComplete;
        protected Unit Unit;
        protected bool IsActive;
        
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
    }
}
