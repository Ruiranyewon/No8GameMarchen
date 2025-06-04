using UnityEngine;
using System.Collections;

public class Scene3KeyPickUp : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerKeyPickUp currentPlayer = null;

    public AudioClip pickupSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (currentPlayer != null)
            {
                currentPlayer.hasKey = true;
                Debug.Log("¿­¼è¸¦ ÁÖ¿ü½À´Ï´Ù!");

                if (pickupSound != null && audioSource != null)
                    audioSource.PlayOneShot(pickupSound);

                StartCoroutine(DestroyAfterDelay(0.3f));
            }
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            currentPlayer = other.GetComponent<PlayerKeyPickUp>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            currentPlayer = null;
        }
    }
}
