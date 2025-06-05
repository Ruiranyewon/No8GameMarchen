using System.Collections;
using UnityEngine;
using TMPro;

public class SceneMessagePopup : MonoBehaviour
{
    public static bool isDialoguePlaying = false;

    public GameObject messagePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public DialogueLine[] lines;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private PlayerMovement playerMovement;

    void Awake()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }
    }

    void Start()
    {
        CharacterDialogueTrigger.isDialoguePlaying = false;
        isDialoguePlaying = true;
        messagePanel.SetActive(false); // Hide initially
        dialogueText.text = "";
        index = 0;
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f); // Wait for 2 seconds
        messagePanel.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            PlayerMovement.canMove = false;
        }
        StartCoroutine(TypeSentence());
    }

    void Update()
    {
        if (messagePanel.activeSelf && Input.GetKeyDown(KeyCode.Return) && !CharacterDialogueTrigger.isDialoguePlaying)
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
                    messagePanel.SetActive(false);
                    dialogueText.text = "";
                    PlayerMovement.canMove = true;
                    isDialoguePlaying = false;
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
