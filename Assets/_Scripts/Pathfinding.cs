using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;

public class Pathfinding : Singleton<Pathfinding>
{
    private const int MOVE_STRAIGHT_CONST = 10;
    private const int MOVE_DIAGONAL_CONST = 14;
    
    [SerializeField] private Transform _gridDebugObjectPrefab;
    [SerializeField] private LayerMask _obstaclesLayerMask;
    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;

    public void Setup(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        
        _gridSystem = new GridSystem<PathNode>(width, height, cellSize, 
            (_, gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);

        const float raycastOffsetDistance = 5f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new(x, z);
                Vector3 woldPos = LevelGrid.Instance.GetWorldPosition(gridPosition);

                if (Physics.Raycast(
                        woldPos + Vector3.down * raycastOffsetDistance,
                        Vector3.up,
                        raycastOffsetDistance * 2f,
                        _obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new();
        List<PathNode> closedList = new();

        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                PathNode pathNode = _gridSystem.GetGridObject(new GridPosition(x, z));

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                //reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                    continue;

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost =
                    currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();
                    
                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_CONST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_CONST * remaining;
    }

    private PathNode GetLowestFCostPathNode(IReadOnlyList<PathNode> pathNodes)
    {
        PathNode lowestFCost = pathNodes[0];

        foreach (PathNode pathNode in pathNodes.Where(pathNode => pathNode.GetFCost() < lowestFCost.GetFCost()))
        {
            lowestFCost = pathNode;
        }

        return lowestFCost;
    }

    private List<PathNode> GetNeighbourList(PathNode node)
    {
        List<PathNode> neighbours = new();

        GridPosition gridPosition = node.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            // left
            neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));  
            if (gridPosition.z - 1 >= 0)
            {
                // left down
                neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            
            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                // left up
                neighbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }
        
        if (gridPosition.x + 1 < _gridSystem.GetWidth())
        {
            // right
            neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));   
            
            if (gridPosition.z - 1 >= 0)
            {
                // right down
                neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            
            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                // right up
                neighbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }
        
        if (gridPosition.z - 1 >= 0)
        {
            // down
            neighbours.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }
        if (gridPosition.z + 1 < _gridSystem.GetHeight())
        {
            // up
            neighbours.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }
        
        return neighbours;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new();
        
        pathNodes.Add(endNode);

        PathNode currentNode = endNode;

        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodes.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodes.Reverse();

        return pathNodes.Select(pathNode => pathNode.GetGridPosition()).ToList();
    }

    private PathNode GetNode(int x, int z) => _gridSystem.GetGridObject(new GridPosition(x, z));
}
