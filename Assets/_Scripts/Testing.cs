using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    
    private void Start()
    {
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.T)) return;

        _unit.GetMoveAction().GetValidActionGridPositionList();
    }
}
