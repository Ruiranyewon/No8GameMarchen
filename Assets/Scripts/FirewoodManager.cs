using UnityEngine;
using TMPro;
using System.Collections;

public class FirewoodManager : MonoBehaviour
{
    public static int firewoodCount = 0;
    public static FirewoodManager instance;

    public TextMeshProUGUI firewoodText; // UI�� ������ �ؽ�Ʈ
    public GameObject firePromptUI;

    private bool hasPrompted = false;
    private bool promptActive = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (promptActive && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("ENTER pressed, hiding firePromptUI");
            firePromptUI.SetActive(false);
            promptActive = false;
        }
    }

    public static void AddFirewood()
    {
        firewoodCount++;
        instance.UpdateFirewoodText();
        Debug.Log("Firewood collected! Total: " + firewoodCount);
    }

    public void UpdateFirewoodText()
    {
        if (firewoodText != null)
        {
            firewoodText.text = $"Firewood: {firewoodCount}/3";
        }

        if (firewoodCount >= 3 && firePromptUI != null && !hasPrompted)
        {
            hasPrompted = true;

            firewoodText.gameObject.SetActive(false);     // �ؽ�Ʈ ����
            firePromptUI.SetActive(true);                 // ������Ʈ ����
            StartCoroutine(HideFirePromptAfterDelay());   // 3�� �� �ڵ� ����
        }
        else
        {
            if (firewoodText != null && !hasPrompted)
                firewoodText.gameObject.SetActive(true);
        }
    }



    public void HideFirewoodText()
    {
        firewoodText.gameObject.SetActive(false);
    }

    private IEnumerator HideFirePromptAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 3�� ��ٸ���
        firePromptUI.SetActive(false);
    }

}
