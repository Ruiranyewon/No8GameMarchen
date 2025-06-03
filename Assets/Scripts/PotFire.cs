using UnityEngine;

public class PotFire : MonoBehaviour
{
    public int requiredFirewood = 3;                   // ���� ���̱� ���� �ʿ��� ���� ����
    public SpriteRenderer potRenderer;                 // �ٲ� ��� ������
    public Sprite fireSprite;                          // �� ���� ���� ��������Ʈ
    private bool activated = false;                    // �ߺ� ����
    private bool playerInRange = false;                // Ʈ���� ���� üũ

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
