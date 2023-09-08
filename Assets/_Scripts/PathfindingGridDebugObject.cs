using Grid;
using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro _gCostText;
    [SerializeField] private TextMeshPro _hCostText;
    [SerializeField] private TextMeshPro _fCostText;
    [SerializeField] private SpriteRenderer _isWalkable;

    private PathNode _pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        
        _pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        
        _gCostText.text = _pathNode.GetGCost().ToString();
        _hCostText.text = _pathNode.GetHCost().ToString();
        _fCostText.text = _pathNode.GetFCost().ToString();
        _isWalkable.color = _pathNode.IsWalkable()
            ? new Color(0.5f, 0.9f, 0.1f, 0.5f)
            : new Color(1f, 0.3f, 0.2f, 0.8f);
    }
}
