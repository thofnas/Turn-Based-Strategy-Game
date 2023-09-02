using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler<Vector3> OnUnitDied;
    public event EventHandler OnUnitDamaged;

    
    [FormerlySerializedAs("_health")] [SerializeField] private int _healthMax = 100;
    private int _health;

    private void Awake()
    {
        _health = _healthMax;
    }

    public float GetHealthNormalized() => Mathf.InverseLerp(0f, _healthMax, _health);
    
    public void Damage(int amount, Vector3 damageDealerPosition)
    {
        _health -= amount;

        OnUnitDamaged?.Invoke(this, EventArgs.Empty);
        
        if (_health <= 0) Die(damageDealerPosition);
    }
    
    private void Die(Vector3 damageDealerPosition)
    {
        OnUnitDied?.Invoke(this, damageDealerPosition);
    }
}
