using System.Collections;
using UnityEngine;
using TMPro;

public class DoorTrigger : MonoBehaviour
{
    public bool hasKey = false; // Set true externally when the player obtains a key
    public static bool isDialoguePlaying = false;
    public GameObject messagePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public DialogueLine[] linesWithKey;
    public DialogueLine[] linesWithoutKey;
    public float typingSpeed = 0.05f;
    private int index = 0;
    private DialogueLine[] linesToUse;
    public SceneSwitch sceneSwitch;

    private bool playerInRange = false;

    private bool isTyping = false;
    private PlayerMovement playerMovement;
    public SpeakerProfile noneSpeakerProfile;

    void Start()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // F키로 대화 시작
        if (playerInRange && !messagePanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(StartDialogue(hasKey));
        }

        // Enter키로 대사 진행
        if (messagePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = linesToUse[index].sentence;
                isTyping = false;
            }
            else
            {
                index++;
                if (index < linesToUse.Length)
                {
                    StartCoroutine(TypeSentence());
                }
                else
                {
                    messagePanel.SetActive(false);
                    dialogueText.text = "";
                    nameText.text = "";
                    if (playerMovement != null)
                        playerMovement.enabled = true;

                    if (hasKey)
                    {
                        Invoke("LoadNextScene", 0.5f);
                    }
                }
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

    IEnumerator StartDialogue(bool keyAvailable)
    {
        yield return new WaitForSeconds(0.5f);
        if (messagePanel != null)
            messagePanel.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        DialogueLine[] currentLines = keyAvailable ? linesWithKey : linesWithoutKey;
        index = 0;

        if (currentLines.Length > 0)
        {
            linesToUse = currentLines;
            StartCoroutine(TypeSentence());
        }
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;

        SpeakerProfile speaker = linesToUse[index].speaker != null ? linesToUse[index].speaker : noneSpeakerProfile;

        nameText.text = speaker.speakerName;
        nameText.color = speaker.speechColor;

        dialogueText.text = "";
        foreach (char letter in linesToUse[index].sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
