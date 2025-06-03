using UnityEngine;
using TMPro;

public class DoorTrigger : MonoBehaviour
{
    public bool hasKey = false; // Set true externally when the player obtains a key
    public GameObject messagePanel;
    public TMP_Text messageText;
    public SceneSwitch sceneSwitch;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (hasKey)
            {
                messageText.text = "문을 열었다.";
                messagePanel.SetActive(true);
                Invoke("LoadNextScene", 1.5f);
            }
            else
            {
                messageText.text = "문이 잠겨있다. 열쇠가 필요한 듯하다.";
                messagePanel.SetActive(true);
                Invoke("HideMessage", 2f);
            }
        }
    }

    void LoadNextScene()
    {
        if (sceneSwitch != null)
        {
            sceneSwitch.LoadScene2();
        }
    }

    void HideMessage()
    {
        messagePanel.SetActive(false);
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
            if (messagePanel != null)
                messagePanel.SetActive(false);
        }
    }
}
