using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    public CanvasGroup normalImageGroup;
    public CanvasGroup blurImageGroup;
    public TextMeshProUGUI introText;

    public float fadeInDuration = 2f;
    public float blurFadeDuration = 2f;
    public float textFadeDuration = 2f;
    public float fadeOutDuration = 2f;

    private bool hasIntroStarted = false;
    private bool canProceed = false;
    private bool hasFadedOut = false;

    void Start()
    {
        normalImageGroup.alpha = 0f;
        blurImageGroup.alpha = 0f;
        introText.alpha = 0f;
        StartCoroutine(FadeInNormalImage());
    }

    void Update()
    {
        if (!hasIntroStarted && Input.GetKeyDown(KeyCode.Space))
        {
            hasIntroStarted = true;
            StartCoroutine(PlayBlurAndText());
        }
        else if (canProceed && !hasFadedOut && Input.GetKeyDown(KeyCode.Space))
        {
            hasFadedOut = true;
            StartCoroutine(FadeOutAll());
        }
    }

    IEnumerator FadeInNormalImage()
    {
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            normalImageGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }
        normalImageGroup.alpha = 1f;
    }

    IEnumerator PlayBlurAndText()
    {
        float timer = 0f;

        while (timer < blurFadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / blurFadeDuration;

            blurImageGroup.alpha = Mathf.Lerp(0f, 1f, t);
            introText.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        blurImageGroup.alpha = 1f;
        introText.alpha = 1f;
        canProceed = true;
    }

    IEnumerator FadeOutAll()
    {
        float timer = 0f;

        float normalStart = normalImageGroup.alpha;
        float blurStart = blurImageGroup.alpha;
        float textStart = introText.alpha;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeOutDuration;

            normalImageGroup.alpha = Mathf.Lerp(normalStart, 0f, t);
            blurImageGroup.alpha = Mathf.Lerp(blurStart, 0f, t);
            introText.alpha = Mathf.Lerp(textStart, 0f, t);
            yield return null;
        }

        normalImageGroup.alpha = 0f;
        blurImageGroup.alpha = 0f;
        introText.alpha = 0f;

        // 다음 씬 전환 등 추가할 경우 여기에
        // SceneManager.LoadScene("MainStageScene");
    }
}
