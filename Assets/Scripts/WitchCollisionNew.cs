using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WitchCollision : MonoBehaviour
{
    public GameObject keyPrefab;
    public AudioClip deathSound; // ���� ���� ����
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // �ݵ�� AudioSource ������Ʈ�� �پ� �־�� ��!
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
        // ���� ���� ���
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // 1�� ��ٸ� �� ���� ����
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

        // Ű ����
        if (keyPrefab != null)
            Instantiate(keyPrefab, new Vector3(49f, -39f, 0f), Quaternion.identity);
    }
}
