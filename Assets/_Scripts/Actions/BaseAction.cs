using System;
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
    }
}
