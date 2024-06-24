using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera will follow (usually the character)
    public float offset; // Offset between the camera and the target
    public float smoothSpeed = 0.125f; // How smoothly the camera follows the target

    void LateUpdate()
    {
        transform.position = target.transform.position + new Vector3(0, 0, offset);
    }
}
