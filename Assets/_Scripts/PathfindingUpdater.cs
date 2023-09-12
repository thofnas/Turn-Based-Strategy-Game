using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void OnDestroy()
    {        
        DestructibleCrate.OnAnyDestroyed -= DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        var destructibleCrate = sender as DestructibleCrate;

        Pathfinding.Instance.SetWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
