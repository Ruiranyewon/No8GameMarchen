using System.Collections;
using UnityEngine;
using TMPro;

public class HandelRemove : MonoBehaviour
{
    [SerializeField] private GameObject handelPrefab;
    [SerializeField] private Transform playerGroupParent;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    public string[] speakerNames;
    [TextArea(2, 5)]
    public string[] sentences;

    public GameObject handelObject;
    public float typingSpeed = 0.05f;

    [Header("Sound")]
    public AudioClip removeSound; // 사운드 추가
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>(); // 없으면 자동 추가

        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
    }

    void Update()
    {
        if (!playerInRange) return;

        if (!dialogueStarted && Input.GetKeyDown(KeyCode.F))
        {
            if (playerController != null && playerController.hasKey)
            {
                StartDialogue();
            }
            else
            {
                Debug.Log("열쇠가 필요합니다.");
            }
        }

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
            if (removeSound != null)
                audioSource.PlayOneShot(removeSound); // 사운드 재생

            Destroy(handelObject); // 소리 재생 후 삭제
            Debug.Log("헨델 오브젝트 삭제됨");
        }

        if (handelPrefab != null && playerGroupParent != null)
        {
            GameObject handelInstance = Instantiate(handelPrefab, playerGroupParent);
            handelInstance.SetActive(false);
            handelInstance.transform.SetSiblingIndex(3);

            Scene3Players playerManager = playerGroupParent.GetComponent<Scene3Players>();
            if (playerManager != null)
            {
                playerManager.UnlockFourthPlayer();
                Debug.Log("헨델 활성화 및 플레이어 전환 대상에 포함됨");
            }
            else
            {
                Debug.LogWarning("Scene3Players 스크립트를 찾을 수 없습니다.");
            }
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
