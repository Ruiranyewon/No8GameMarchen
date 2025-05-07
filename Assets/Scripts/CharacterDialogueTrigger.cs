using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CharacterDialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel; // Inspector에 UI Panel 연결
    public TextMeshProUGUI dialogueText;
    [TextArea(2, 5)] public string[] dialogueLines;
    public float typingSpeed = 0.05f;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool cancelTyping = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true);
            PlayerMovement.canMove = false;
            currentLineIndex = 0;
            StartCoroutine(TypeLine());
        }
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                cancelTyping = true;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < dialogueLines.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    dialoguePanel.SetActive(false);
                    dialogueText.text = "";
                    PlayerMovement.canMove = true;
                }
            }
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        cancelTyping = false;
        dialogueText.text = "";

        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            if (cancelTyping)
            {
                dialogueText.text = dialogueLines[currentLineIndex];
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        cancelTyping = false;
    }
}