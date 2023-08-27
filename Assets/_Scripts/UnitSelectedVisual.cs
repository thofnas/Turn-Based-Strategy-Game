using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnityActionSystem_OnSelectedUnitChanged;
        
        UpdateVisual();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnityActionSystem_OnSelectedUnitChanged;
    }

    private void UpdateVisual() => 
        _meshRenderer.enabled = UnitActionSystem.Instance.GetSelectedUnit() == _unit;

    private void UnityActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) => 
        UpdateVisual();
}
