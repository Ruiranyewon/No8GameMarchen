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
                // 스페이스 누르기 전까진 대기
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
                    // 아직 스페이스 안 누름 → 진행 막기
                    canProceed = false;
                }
                return;
            }
        }

        // 그 외 라인은 무제한 진행 허용
        canProceed = true;
    }
}
