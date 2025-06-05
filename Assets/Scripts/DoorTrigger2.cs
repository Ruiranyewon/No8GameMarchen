using UnityEngine;

public class DoorTrigger2 : MonoBehaviour
{
    public SceneSwitch sceneSwitch;

    private bool playerInRange = false;

    void Update()
    {
        // F키를 누르면 즉시 씬 전환
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (sceneSwitch != null)
        {
            sceneSwitch.LoadScene3();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
