using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorDarkenOnHandel : MonoBehaviour
{
    public string requiredTag = "Handel";        // �ݵ�� "Handel" �±׸� �ν�
    public string nextSceneName = "Scene4";
    public float fadeDuration = 2f;

    private SpriteRenderer spriteRenderer;
    private bool isPlayerInRange = false;
    private GameObject playerInRange;
    private bool alreadyActivated = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("�� ������Ʈ���� SpriteRenderer�� �����ϴ�!");
    }

    void Update()
    {
        if (!isPlayerInRange || playerInRange == null || alreadyActivated)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            // "Handel" �±��� ��쿡�� �۵�
            if (playerInRange.CompareTag(requiredTag))
            {
                alreadyActivated = true;
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
