using System.Collections.Generic;
using Grid;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    
    private void Start()
    {
        GridSystemVisual.Instance.HideAllGridPositions();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.T)) return;

        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        GridPosition startGridPosition = new(0, 0);
        
        List<GridPosition> gridPositions = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

        for (int index = 0; index < gridPositions.Count - 1; index++)   
        {
            Debug.DrawLine(
                LevelGrid.Instance.GetWorldPosition(gridPositions[index]), 
                LevelGrid.Instance.GetWorldPosition(gridPositions[index + 1]),
                Color.white, 
                10f, true);
        }
    }
}
