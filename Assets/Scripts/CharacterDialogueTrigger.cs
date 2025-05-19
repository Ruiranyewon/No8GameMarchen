using System.Collections;
using UnityEngine;
using TMPro;

public class CharacterDialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText; // TextMeshPro 텍스트
    public string[] sentences; // Inspector에서 대사 입력
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private bool dialogueStarted = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dialogueStarted)
        {
            dialogueStarted = true;
            dialoguePanel.SetActive(true);
            StartCoroutine(TypeSentence());
        }
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = sentences[index];
                isTyping = false;
            }
            else
            {
                index++;
                if (index < sentences.Length)
                {
                    StartCoroutine(TypeSentence());
                }
                else
                {
                    dialoguePanel.SetActive(false);
                    dialogueText.text = "";
                    index = 0;
                    dialogueStarted = false;
                }
            }
        }
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}