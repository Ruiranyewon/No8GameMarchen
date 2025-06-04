using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (플레이어)
    public Vector3 offset = new Vector3(0, 0, -10); // 기본 카메라 오프셋

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
