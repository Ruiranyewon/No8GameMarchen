using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorDarkenOnHandel : MonoBehaviour
{
    public string requiredTag = "Handel";       // "Handel" 태그 캐릭터만 인식
    public string nextSceneName = "Scene4";
    public float fadeDuration = 2f;

    public AudioClip doorSound;                 //  사운드 추가
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private bool isPlayerInRange = false;
    private GameObject playerInRange;
    private bool alreadyActivated = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("이 오브젝트에는 SpriteRenderer가 없습니다!");

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

                // 소리 재생
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
