using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CharacterDialogueTrigger : MonoBehaviour
{
    public static bool isDialoguePlaying = false;

    [SerializeField] private GameObject bifurcation;
    [SerializeField] private int sceneDialogueLimit = 12;

    private PlayerMovement playerMovement;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText; // TextMeshPro 텍스트
    public TMP_Text nameText;
    public DialogueLine[] lines;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private bool dialogueStarted = false;
    private bool playerInRange = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();

        if (SceneManager.GetActiveScene().name != "Scene1")
        {
            bifurcation = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Update()
    {
        if (!dialogueStarted && playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            dialogueStarted = true;
            isDialoguePlaying = true;
            SceneMessagePopup.isDialoguePlaying = true;
            dialoguePanel.SetActive(true);
            PlayerMovement.canMove = false;
            StartCoroutine(TypeSentence());
        }
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                if (index < lines.Length)
                {
                    dialogueText.text = lines[index].sentence;
                }
                isTyping = false;
            }
            else
            {
                index++;

                int dialogueLimit = sceneDialogueLimit;
                if (bifurcation != null && !bifurcation.activeInHierarchy)
                    dialogueLimit = lines.Length;

                if (index < dialogueLimit)
                {
                    if (index < lines.Length)
                    {
                        StartCoroutine(TypeSentence());
                    }
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;

        if (lines[index].speaker != null)
        {
            nameText.text = lines[index].speaker.speakerName;
            nameText.color = lines[index].speaker.speechColor;
        }
        else
        {
            nameText.text = " ";
            nameText.color = Color.white;
        }

        string fullSentence = lines[index].sentence;
        dialogueText.text = "";

        for (int i = 0; i < fullSentence.Length; i++)
        {
            dialogueText.text += fullSentence[i];
            if (Input.GetKeyDown(KeyCode.Return))
            {
                dialogueText.text = fullSentence;
                break;
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void StartDialogueExternally()
    {
        if (!dialogueStarted)
        {
            dialogueStarted = true;
            isDialoguePlaying = true;
            SceneMessagePopup.isDialoguePlaying = true;
            dialoguePanel.SetActive(true);
            PlayerMovement.canMove = false;
            StartCoroutine(TypeSentence());
        }
    }
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        index = 0;
        dialogueStarted = false;

        isDialoguePlaying = false;
        SceneMessagePopup.isDialoguePlaying = false;

        PlayerMovement.canMove = true;
    }
}