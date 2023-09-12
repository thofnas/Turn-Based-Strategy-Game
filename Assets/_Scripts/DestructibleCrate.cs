using System;
using Grid;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;
    
    private GridPosition _gridPosition;
    
    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() => _gridPosition;

    public void Damage()
    {
        Destroy(gameObject);
        
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }
}
