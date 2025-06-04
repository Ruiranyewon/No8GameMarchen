using UnityEngine;

public class FInteractFilter : MonoBehaviour
{
    public string requiredPlayerName = "Marin";  // 상호작용 가능한 캐릭터 이름

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObj = other.gameObject;

            // 현재 활성화된 플레이어의 이름이 Marin인지 확인
            if (playerObj.name == requiredPlayerName)
            {
                // FInteractDialogue 활성화
                GetComponent<FInteractDialogue>().enabled = true;
            }
            else
            {
                // 비활성화
                GetComponent<FInteractDialogue>().enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 범위 벗어나면 꺼둬도 무방
            GetComponent<FInteractDialogue>().enabled = false;
        }
    }
}
