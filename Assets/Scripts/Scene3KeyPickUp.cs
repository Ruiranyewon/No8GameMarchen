using UnityEngine;

public class Scene3KeyPickUp : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerKeyPickUp currentPlayer = null;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (currentPlayer != null)
            {
                currentPlayer.hasKey = true;
                Debug.Log("¿­¼è¸¦ ÁÖ¿ü½À´Ï´Ù!");
                Destroy(gameObject);
            }
        }
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
