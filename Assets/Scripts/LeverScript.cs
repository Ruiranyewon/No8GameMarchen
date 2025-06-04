using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public GameObject puzzleUI; // 문제 UI
    public TMP_Text inputDisplay; // 숫자 표시용
    public GameObject leverWithLock; // 잠긴 상태 오브젝트
    public GameObject leverOpenLock; // 열린 상태 오브젝트
    public GameObject prison; // 비활성화 대상
    public CharacterDialogueTrigger dialogueTriggerAfterUnlock;
    public GameObject playerTrapped;

    private int[] digits = new int[4];
    private int selectedIndex = 0;
    private bool isPuzzleActive = false;
    private PlayerMovement playerMovement;

    void Update()
    {
        if (!isPuzzleActive) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = (selectedIndex + 3) % 4; // move left
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = (selectedIndex + 1) % 4; // move right
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            digits[selectedIndex] = (digits[selectedIndex] + 1) % 10;
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            digits[selectedIndex] = (digits[selectedIndex] + 9) % 10; // decrement with wrap
            UpdateDisplay();
        }

        if (digits[0] == 7 && digits[1] == 0 && digits[2] == 2 && digits[3] == 5)
        {
            CompletePuzzle();
        }
    }

    private void UpdateDisplay()
    {
        string display = "";
        for (int i = 0; i < 4; i++)
        {
            if (i == selectedIndex)
                display += $"<color=yellow><b>{digits[i]}</b></color>";
            else
                display += digits[i].ToString();
        }
        inputDisplay.text = display;
    }

    private void CompletePuzzle()
    {
        isPuzzleActive = false;
        puzzleUI.SetActive(false);
        leverWithLock.SetActive(false);
        leverOpenLock.SetActive(true);
        StartCoroutine(DisablePrison());
        if (playerMovement != null)
            playerMovement.enabled = true;
        if (dialogueTriggerAfterUnlock != null)
            dialogueTriggerAfterUnlock.StartDialogueExternally();
    }

    private IEnumerator DisablePrison()
    {
        yield return new WaitForSeconds(1.5f);
        if (prison != null)
            prison.SetActive(false);
        playerTrapped.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            puzzleUI.SetActive(true);
            for (int i = 0; i < 4; i++) digits[i] = 0;
            selectedIndex = 0;
            UpdateDisplay();
            isPuzzleActive = true;
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            puzzleUI.SetActive(false);
            isPuzzleActive = false;
            if (playerMovement != null)
                playerMovement.enabled = true;
        }
    }
}
