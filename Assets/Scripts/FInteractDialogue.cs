using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueLine_FInteract
{
    public string speakerName;
    [TextArea(2, 5)]
    public string sentence;
}

public class FInteractDialogue : MonoBehaviour
{
    public bool IsDialogueActive() => dialogueStarted;
    public int CurrentLineIndex() => index;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public DialogueLine_FInteract[] lines;
    public float typingSpeed = 0.05f;
    public float activationRange = 2f;

    private static bool isAnyDialogueActive = false;

    private int index = 0;
    private bool isTyping = false;
    private bool dialogueStarted = false;
    private bool dialogueFinished = false;
    private bool playerInRange = false;

    private Transform player;
    private PlayerMovement playerMovement;
    private Animator playerAnimator;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            playerAnimator = playerObj.GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if ((playerInRange || dist <= activationRange) && !dialogueStarted && !dialogueFinished && !isAnyDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartDialogue();
            }
        }

        if (dialogueStarted && dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
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
                    EndDialogue();
                }
            }
        }
    }

    void StartDialogue()
    {
        isAnyDialogueActive = true;
        dialogueStarted = true;
        dialoguePanel.SetActive(true);
        if (playerMovement != null)
            playerMovement.enabled = false;
        if (playerAnimator != null)
            playerAnimator.speed = 0f;

        index = 0;
        StartCoroutine(TypeSentence());
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
        index = 0;
        dialogueStarted = false;
        dialogueFinished = true;
        isAnyDialogueActive = false;

        if (playerMovement != null)
            playerMovement.enabled = true;
        if (playerAnimator != null)
            playerAnimator.speed = 1f;
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        nameText.text = lines[index].speakerName;
        nameText.ForceMeshUpdate();

        dialogueText.text = "";
        dialogueText.ForceMeshUpdate();

        foreach (char letter in lines[index].sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
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
