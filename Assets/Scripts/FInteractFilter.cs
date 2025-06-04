using UnityEngine;

public class FInteractFilter : MonoBehaviour
{
    public string requiredPlayerName = "Marin";  // ��ȣ�ۿ� ������ ĳ���� �̸�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerObj = other.gameObject;

            // ���� Ȱ��ȭ�� �÷��̾��� �̸��� Marin���� Ȯ��
            if (playerObj.name == requiredPlayerName)
            {
                // FInteractDialogue Ȱ��ȭ
                GetComponent<FInteractDialogue>().enabled = true;
            }
            else
            {
                // ��Ȱ��ȭ
                GetComponent<FInteractDialogue>().enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ���� ����� ���ֵ� ����
            GetComponent<FInteractDialogue>().enabled = false;
        }
    }
}
