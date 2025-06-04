using UnityEngine;

public class FirewoodPickup : MonoBehaviour
{
    private bool playerInRange = false;
    private bool collected = false;

    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInRange && !collected && Input.GetKeyDown(KeyCode.F))
        {
            Collect();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Collect()
    {
        collected = true;

        if (audioSource != null && pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        FirewoodManager.AddFirewood();

        Destroy(gameObject, 0.3f); // 家府 犁积 场唱绊 昏力
    }
}
