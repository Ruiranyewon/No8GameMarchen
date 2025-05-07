using UnityEngine;

public class CharacterDialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel; // Inspector에 UI Panel 연결

    void Start()
    {
        dialoguePanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true);
        }
    }
}