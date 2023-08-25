using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld s_instance;
    
    [SerializeField] private LayerMask _floorLayerMask;

    private void Awake()
    {
        s_instance = this;
    }
    
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, s_instance._floorLayerMask);
        
        return raycastHit.point;
    }
}
