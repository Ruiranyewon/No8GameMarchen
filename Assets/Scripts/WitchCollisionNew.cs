using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WitchCollision : MonoBehaviour
{
    public GameObject keyPrefab;
    public AudioClip deathSound; // 마녀 죽음 사운드
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 반드시 AudioSource 컴포넌트가 붙어 있어야 함!
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOver");
        }
        else if (other.CompareTag("FirePot"))
        {
            var ai = GetComponent<WitchAINew>();
            if (ai != null)
                ai.StopMovement();

            StartCoroutine(HandleWitchDeath());
        }
    }

    IEnumerator HandleWitchDeath()
    {
        // 사운드 먼저 재생
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // 1초 기다린 후 마녀 삭제
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

        // 키 생성
        if (keyPrefab != null)
            Instantiate(keyPrefab, new Vector3(49f, -39f, 0f), Quaternion.identity);
    }
}
