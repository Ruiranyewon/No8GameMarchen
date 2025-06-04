using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorDarkenOnHandel : MonoBehaviour
{
    public string requiredTag = "Handel";       // "Handel" �±� ĳ���͸� �ν�
    public string nextSceneName = "Scene4";
    public float fadeDuration = 2f;

    public AudioClip doorSound;                 //  ���� �߰�
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private bool isPlayerInRange = false;
    private GameObject playerInRange;
    private bool alreadyActivated = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("�� ������Ʈ���� SpriteRenderer�� �����ϴ�!");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (!isPlayerInRange || playerInRange == null || alreadyActivated)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerInRange.CompareTag(requiredTag))
            {
                alreadyActivated = true;

                // �Ҹ� ���
                if (doorSound != null)
                    audioSource.PlayOneShot(doorSound);

                StartCoroutine(FadeToBlackAndLoad());
            }
        }
    }

    IEnumerator FadeToBlackAndLoad()
    {
        Color originalColor = spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            spriteRenderer.color = Color.Lerp(originalColor, Color.black, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nextSceneName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(requiredTag))
        {
            isPlayerInRange = true;
            playerInRange = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(requiredTag) && other.gameObject == playerInRange)
        {
            isPlayerInRange = false;
            playerInRange = null;
        }
    }
}
