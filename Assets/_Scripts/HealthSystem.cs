using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler<Vector3> OnUnitDied;
    
    [SerializeField] private int _health = 100;

    public void Damage(int amount, Vector3 damageDealerPosition)
    {
        _health -= amount;

        if (_health <= 0) Die(damageDealerPosition);
        
        print(_health);
    }
    
    private void Die(Vector3 damageDealerPosition)
    {
        OnUnitDied?.Invoke(this, damageDealerPosition);
    }
}
