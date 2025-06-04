using UnityEngine;
using UnityEngine.UI;

public class EyeFollowUI : MonoBehaviour
{
    public RectTransform pupil;         // pupil 이미지
    public RectTransform eyeArea;       // 눈 전체 영역 (부모)

    public float maxDistance = 10f;     // 눈동자가 움직일 수 있는 최대 거리 (픽셀 단위)

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            eyeArea, Input.mousePosition, null, out mousePos
        );

        Vector2 clampedPos = Vector2.ClampMagnitude(mousePos, maxDistance);
        pupil.anchoredPosition = clampedPos;
    }
}
