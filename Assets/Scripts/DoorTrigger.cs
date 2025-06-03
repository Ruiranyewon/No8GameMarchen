using UnityEngine;
using TMPro;

public class DoorTrigger : MonoBehaviour
{
    public bool hasKey = false; // Set true externally when the player obtains a key
    public GameObject messagePanel;
    public TMP_Text messageText;
    public SceneSwitch sceneSwitch;

    private bool playerInRange = false;

    void Start()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (hasKey)
            {
                Debug.Log("문 열기 가능 : 씬 전환 가능");
                messageText.text = "You opened the door.";
                messagePanel.SetActive(true);
                Invoke("LoadNextScene", 0.5f); // Add delay before scene load
            }
            else
            {
                messageText.text = "The door is locked. You need a key.";
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
