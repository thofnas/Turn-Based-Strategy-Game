using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    [SerializeField] private Mode _mode;

    private void LateUpdate() {
        if (Camera.main is null) return;

        switch (_mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                var position = transform.position;
                var dirFromCamera = position - Camera.main.transform.position;
                transform.LookAt(position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }
}
