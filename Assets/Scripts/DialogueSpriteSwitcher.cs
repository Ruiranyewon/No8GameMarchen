using UnityEngine;

public class DialogueSpriteSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class SpriteChangeEvent
    {
        public int lineIndex;                          // �� ��° ���� �� �ٲ���
        public Sprite newSprite;                       // �ٲ� ��������Ʈ
        public SpriteRenderer targetRenderer;          // �ٲ� ������Ʈ
    }

    public FInteractDialogue dialogueScript;           // ����� ��ȭ ��ũ��Ʈ
    public SpriteChangeEvent[] spriteChangeEvents;     // �̺�Ʈ �迭

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
                    hasChanged[i] = true; // �� ���� ����ǵ���
                }
            }
        }
    }
}
