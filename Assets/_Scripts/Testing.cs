using System;
using Grid;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObjectPrefab;
    private GridSystem _gridSystem;
    
    private void Start()
    {
        _gridSystem = new GridSystem(10, 10);
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }

    private void Update()
    {
        //print(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
