using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const float TOLERANCE = 0.1f;
    private const float MOVE_SPEED = 4f;
    private const float ROTATE_SPEED = 10f;
    
    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));

    [SerializeField] private Animator _unitAnimator;
    private Vector3 _targetPosition;
    
    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        
        if (Vector3.Distance(transform.position, _targetPosition) <= TOLERANCE)
        {
            transform.position = _targetPosition;
            _unitAnimator.SetBool(IsWalking, false);
            return;
        }

        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        transform.position += moveDirection * (Time.deltaTime * MOVE_SPEED);
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * ROTATE_SPEED);
        
        _unitAnimator.SetBool(IsWalking, true);
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
