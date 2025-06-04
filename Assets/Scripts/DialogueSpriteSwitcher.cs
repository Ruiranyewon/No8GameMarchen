using UnityEngine;

public class DialogueSpriteSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class SpriteChangeEvent
    {
        public int lineIndex;
        public Sprite newSprite;
        public SpriteRenderer targetRenderer;
        public AudioClip soundBeforeChange;
    }

    public FInteractDialogue dialogueScript;
    public SpriteChangeEvent[] spriteChangeEvents;
    public AudioSource audioSource;

    private bool[] hasChanged;

    public static bool canProceed = false;

    void Start()
    {
        hasChanged = new bool[spriteChangeEvents.Length];
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayer == null || currentPlayer.name != "Marin") return;
        if (dialogueScript == null || !dialogueScript.IsDialogueActive()) return;

        int currentIndex = dialogueScript.CurrentLineIndex();

        for (int i = 0; i < spriteChangeEvents.Length; i++)
        {
            if (!hasChanged[i] && currentIndex == spriteChangeEvents[i].lineIndex)
            {
                // �����̽� ������ ������ ���
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    var evt = spriteChangeEvents[i];

                    if (evt.soundBeforeChange != null && audioSource != null)
                        audioSource.PlayOneShot(evt.soundBeforeChange);

                    if (evt.targetRenderer != null && evt.newSprite != null)
                        evt.targetRenderer.sprite = evt.newSprite;

                    hasChanged[i] = true;
                    canProceed = true;
                }
                else
                {
                    // ���� �����̽� �� ���� �� ���� ����
                    canProceed = false;
                }
                return;
            }
        }

        // �� �� ������ ������ ���� ���
        canProceed = true;
    }
}
