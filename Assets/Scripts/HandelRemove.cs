using System.Collections;
using UnityEngine;
using TMPro;

public class HandelRemove : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    public string[] speakerNames;
    [TextArea(2, 5)]
    public string[] sentences;

    public GameObject handelObject;
    public float typingSpeed = 0.05f;

    private bool playerInRange = false;
    private int index = 0;
    private bool isTyping = false;
    private bool dialogueStarted = false;

    private PlayerKeyPickUp playerController;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerController = playerObj.GetComponent<PlayerKeyPickUp>();

        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
    }

    void Update()
    {
        if (!playerInRange) return;

        // F Ű�� ��ȭ ����
        if (!dialogueStarted && Input.GetKeyDown(KeyCode.F))
        {
            if (playerController != null && playerController.hasKey)
            {
                StartDialogue();
            }
            else
            {
                Debug.Log("���谡 �ʿ��մϴ�.");
            }
        }

        // ��ȭ ���� (F �Ǵ� Enter��)
        if (dialogueStarted && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return)))
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
                    StartCoroutine(TypeLine());
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
        dialogueStarted = true;
        dialoguePanel.SetActive(true);
        index = 0;
        StartCoroutine(TypeLine());
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
        dialogueStarted = false;

        if (handelObject != null)
        {
            Destroy(handelObject);
            Debug.Log("� ������Ʈ ������");
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        nameText.text = speakerNames[index];
        dialogueText.text = "";

        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
