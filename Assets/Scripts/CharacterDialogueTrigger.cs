using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public SpeakerProfile speaker;
    [TextArea(2, 5)]
    public string sentence;
}

[System.Serializable]
[CreateAssetMenu(fileName = "SpeakerProfile", menuName = "Dialogue/Speaker Profile")]
public class SpeakerProfile : ScriptableObject
{
    public string speakerName;
    public Color speechColor;
}

public class CharacterDialogueTrigger : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText; // TextMeshPro 텍스트
    public TMP_Text nameText;
    public DialogueLine[] lines;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private bool dialogueStarted = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dialogueStarted)
        {
            dialogueStarted = true;
            dialoguePanel.SetActive(true);
            if (playerMovement != null)
                playerMovement.enabled = false;
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
                dialogueText.text = lines[index].sentence;
                isTyping = false;
            }
            else
            {
                index++;
                if (index < lines.Length)
                {
                    StartCoroutine(TypeSentence());
                }
                else
                {
                    dialoguePanel.SetActive(false);
                    dialogueText.text = "";
                    index = 0;
                    dialogueStarted = false;

                    if (playerMovement != null)
                        playerMovement.enabled = true;
                }
            }
        }
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        nameText.text = lines[index].speaker.speakerName;
        nameText.color = lines[index].speaker.speechColor;

        dialogueText.text = "";
        foreach (char letter in lines[index].sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}