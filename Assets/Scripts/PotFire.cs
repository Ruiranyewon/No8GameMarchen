using UnityEngine;
using UnityEngine.Rendering.Universal;  // 2D Light ����� ���� ���ӽ����̽�

public class PotFire : MonoBehaviour
{
    public AudioClip fireSound;         // ĥ���� �� ���� �� ����
    public AudioClip witchHitSound;     // ���� ��Ÿ�� �Ҹ�
    public AudioClip witchScreamSound;  // ���� ��� ����
    private AudioSource audioSource;

    public int requiredFirewood = 3;
    public SpriteRenderer potRenderer;
    public Sprite fireSprite;
    public Light2D fireLight;

    public GameObject firePromptUI;

    private bool activated = false;
    private bool playerInRange = false;
    private GameObject currentPlayer;

    void Start()
    {
        if (fireLight != null)
            fireLight.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (activated || !playerInRange || currentPlayer == null) return;

        if (currentPlayer.name == "Chill" &&
            Input.GetKeyDown(KeyCode.Space) &&
            FirewoodManager.firewoodCount >= requiredFirewood)
        {
            if (potRenderer != null && fireSprite != null)
            {
                potRenderer.sprite = fireSprite;
                gameObject.tag = "FirePot";
                activated = true;
                Debug.Log("Pot activated by Chill!");

                if (fireLight != null)
                    fireLight.enabled = true;

                if (fireSound != null)
                    audioSource.PlayOneShot(fireSound);

                // firePromptUI ���� ���� (Ȱ��ȭ ������ ��츸)
                if (firePromptUI != null && firePromptUI.activeSelf)
                {
                    firePromptUI.SetActive(false);
                    Debug.Log("firePromptUI turned off by PotFire.");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.gameObject;
            playerInRange = true;
        }

        if (other.CompareTag("Witch"))
        {
            // ���ÿ� �� �Ҹ� ���
            if (witchHitSound != null)
                audioSource.PlayOneShot(witchHitSound);

            if (witchScreamSound != null)
                audioSource.PlayOneShot(witchScreamSound);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            currentPlayer = null;
        }
    }
}
