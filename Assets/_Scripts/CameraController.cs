using System;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CinemachineTransposer _cinemachineTransposer;
    private Vector3 _targetFollowOffset;
    
    private void Start()
    {
        _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputMoveDir.z++;
        if (Input.GetKey(KeyCode.A)) inputMoveDir.x--;
        if (Input.GetKey(KeyCode.S)) inputMoveDir.z--;
        if (Input.GetKey(KeyCode.D)) inputMoveDir.x++;

        const float moveSpeed = 10f;
        Vector3 moveVector = (transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x).normalized;
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
    }
    
    private void HandleRotation()
    {
        const float rotationSpeed = 100f;
        Vector3 rotationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q)) rotationVector.y++;
        if (Input.GetKey(KeyCode.E)) rotationVector.y--;

        transform.eulerAngles += rotationVector * (rotationSpeed * Time.deltaTime);
    } 
    
    private void HandleZoom()
    {
        const float zoomAmount = 1f;
        const float zoomSpeed = 5f;

        if (Input.mouseScrollDelta.y > 0) _targetFollowOffset.y -= zoomAmount;
        if (Input.mouseScrollDelta.y < 0) _targetFollowOffset.y += zoomAmount;

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
