using UnityEngine;
using UnityEngine.Rendering.Universal;  // 2D Light 사용을 위한 네임스페이스

public class PotFire : MonoBehaviour
{
    public int requiredFirewood = 3;
    public SpriteRenderer potRenderer;
    public Sprite fireSprite;
    public Light2D fireLight;  

    private bool activated = false;
    private bool playerInRange = false;
    private GameObject currentPlayer;

    void Start()
    {
        if (fireLight != null)
            fireLight.enabled = false;  
    }

    void Update()
    {
        if (activated || !playerInRange || currentPlayer == null) return;

        if (currentPlayer.name == "Chill" &&
            Input.GetKeyDown(KeyCode.F) &&
            FirewoodManager.firewoodCount >= requiredFirewood)
        {
            if (potRenderer != null && fireSprite != null)
            {
                potRenderer.sprite = fireSprite;
                gameObject.tag = "FirePot";
                activated = true;
                Debug.Log("Pot activated by Chill!");

                if (fireLight != null)
                    fireLight.enabled = true;  
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.gameObject;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            currentPlayer = null;
        }
    }
}
