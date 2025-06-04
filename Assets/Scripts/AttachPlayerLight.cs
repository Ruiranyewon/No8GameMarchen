using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttachPlayerLight : MonoBehaviour
{
    void Start()
    {
        // 이미 Light2D가 자식에 있으면 중복 생성 방지
        Light2D existingLight = GetComponentInChildren<Light2D>();
        if (existingLight != null) return;

        GameObject lightObj = new GameObject("PlayerLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = Vector3.zero;

        Light2D light2D = lightObj.AddComponent<Light2D>();
        light2D.lightType = Light2D.LightType.Point;
        light2D.intensity = 0.5f;
        light2D.pointLightInnerRadius = 25.0f;
        light2D.pointLightOuterRadius = 50.0f;
        light2D.color = Color.white;
        light2D.falloffIntensity = 1f;
        light2D.shadowIntensity = 0f;
    }
}
