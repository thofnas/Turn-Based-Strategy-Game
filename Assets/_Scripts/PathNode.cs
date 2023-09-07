using Grid;

public class PathNode
{
    private readonly GridPosition _gridPosition;
    private int _g;
    private int _h;
    private int _f;
    private bool _isWalkable = true;
    private PathNode _cameFrom;
    
    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }
    
    public override string ToString() => $"{_gridPosition}";

    public int GetGCost() => _g;
    
    public int GetHCost() => _h;
    
    public int GetFCost() => _f;
    
    public void SetGCost(int g) => _g = g;
    
    public void SetHCost(int h) => _h = h;
    
    public void CalculateFCost() => _f = _g + _h;

    public void ResetCameFromPathNode() => _cameFrom = null;
    
    public GridPosition GetGridPosition() => _gridPosition;

    public void SetCameFromPathNode(PathNode pathNode) => _cameFrom = pathNode;
    
    public PathNode GetCameFromPathNode() => _cameFrom;

    public bool IsWalkable() => _isWalkable;

    public void SetIsWalkable(bool value) => _isWalkable = value;
}
