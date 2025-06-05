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

    private bool isPrison => gameObject.name == "prison";



    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            // prison이면 이름 무관, 그 외는 Marin만
            if (isPrison || playerObj.name == "Marin")
            {
                player = playerObj.transform;
                playerMovement = playerObj.GetComponent<PlayerMovement>();
                playerAnimator = playerObj.GetComponent<Animator>();
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if ((playerInRange || dist <= activationRange) && !dialogueStarted && !dialogueFinished && !isAnyDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.F) &&
                (isPrison || player.name == "Marin"))
            {
                StartDialogue();
            }
        }

        if (dialogueStarted && dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (!isPrison && !DialogueSpriteSwitcher.canProceed) return;

            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index].sentence;
                isTyping = false;
            }
            else
            {
                index++;
                DialogueSpriteSwitcher.canProceed = false;

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

        // 무조건 정지 처리 (prison이든 Marin이든)
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

        if (lines.Length > 0 &&
            lines[lines.Length - 1].sentence.Trim() == "(Logs are scattered around the map)" &&
            FirewoodManager.instance != null)
        {
            FirewoodManager.instance.UpdateFirewoodText();
        }
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
        if (other.CompareTag("Player") &&
            (isPrison || other.name == "Marin"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            (isPrison || other.name == "Marin"))
        {
            playerInRange = false;
        }
    }
}
