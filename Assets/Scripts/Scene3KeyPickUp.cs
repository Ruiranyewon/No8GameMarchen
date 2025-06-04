using UnityEngine;

public class Scene3KeyPickUp : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PlayerKeyPickUp player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerKeyPickUp>();
            if (player != null)
            {
                player.hasKey = true;
                Debug.Log("¿­¼è¸¦ ÁÖ¿ü½À´Ï´Ù!");
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}

