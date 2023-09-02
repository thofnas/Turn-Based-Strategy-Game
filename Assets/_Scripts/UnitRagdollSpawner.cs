using System;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private UnitRagdoll _ragdollPrefab;
    [SerializeField] private Transform _originalRootBone;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        
        _healthSystem.OnUnitDied += HealthSystem_OnUnitDied;
    }

    private void OnDestroy()
    {
        _healthSystem.OnUnitDied -= HealthSystem_OnUnitDied;
    }

    private void HealthSystem_OnUnitDied(object sender, Vector3 damageDealerPosition)
    {
        UnitRagdoll unitRagdoll = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        unitRagdoll.Setup(_originalRootBone, damageDealerPosition);
    }
}
