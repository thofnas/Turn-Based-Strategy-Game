using UnityEngine;

public class Unit : MonoBehaviour
{
    private const float TOLERANCE = 0.1f;
    private const float MOVE_SPEED = 4;
    
    private Vector3 _targetPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Move(MouseWorld.GetPosition());
        
        if (Vector3.Distance(transform.position, _targetPosition) <= TOLERANCE)
        {
            transform.position = _targetPosition;
            return;
        }

        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        transform.position += moveDirection * (Time.deltaTime * MOVE_SPEED);
    }

    private void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
