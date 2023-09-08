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
    }
}
