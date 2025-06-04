using UnityEngine;

public class PotFire : MonoBehaviour
{
    public int requiredFirewood = 3;                   // 불을 붙이기 위해 필요한 장작 개수
    public SpriteRenderer potRenderer;                 // 바꿀 대상 렌더러
    public Sprite fireSprite;                          // 불 붙은 상태 스프라이트
    private bool activated = false;                    // 중복 방지
    private bool playerInRange = false;                // 트리거 범위 체크

    void Update()
    {
        if (activated || !playerInRange) return;

        if (Input.GetKeyDown(KeyCode.F) && FirewoodManager.firewoodCount >= requiredFirewood)
        {
            if (potRenderer != null && fireSprite != null)
            {
                potRenderer.sprite = fireSprite;
                activated = true;
                Debug.Log("Pot activated!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
