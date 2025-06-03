using UnityEngine;

public class DialogueSpriteSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class SpriteChangeEvent
    {
        public int lineIndex;                          // 몇 번째 줄일 때 바꿀지
        public Sprite newSprite;                       // 바꿀 스프라이트
        public SpriteRenderer targetRenderer;          // 바꿀 오브젝트
    }

    public FInteractDialogue dialogueScript;           // 연결된 대화 스크립트
    public SpriteChangeEvent[] spriteChangeEvents;     // 이벤트 배열

    private bool[] hasChanged;

    void Start()
    {
        hasChanged = new bool[spriteChangeEvents.Length];
    }

    void Update()
    {
        if (dialogueScript == null || !dialogueScript.IsDialogueActive()) return;

        int currentIndex = dialogueScript.CurrentLineIndex();

        for (int i = 0; i < spriteChangeEvents.Length; i++)
        {
            if (!hasChanged[i] && currentIndex == spriteChangeEvents[i].lineIndex)
            {
                var evt = spriteChangeEvents[i];
                if (evt.targetRenderer != null && evt.newSprite != null)
                {
                    evt.targetRenderer.sprite = evt.newSprite;
                    hasChanged[i] = true; // 한 번만 변경되도록
                }
            }
        }
    }
}
