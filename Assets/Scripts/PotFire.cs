using UnityEngine;
using UnityEngine.Rendering.Universal;  // 2D Light 사용을 위한 네임스페이스

public class PotFire : MonoBehaviour
{
    public AudioClip fireSound;         // 칠리로 불 붙일 때 사운드
    public AudioClip witchHitSound;     // 마녀 불타는 소리
    public AudioClip witchScreamSound;  // 마녀 비명 사운드
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

                // firePromptUI 직접 끄기 (활성화 상태일 경우만)
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
            // 동시에 두 소리 재생
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
