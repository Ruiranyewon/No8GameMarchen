using UnityEngine;

public class FirewoodPickup : MonoBehaviour
{
    private bool playerInRange = false;
    private bool collected = false;

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
        FirewoodManager.AddFirewood();
        Destroy(gameObject);
    }
}
