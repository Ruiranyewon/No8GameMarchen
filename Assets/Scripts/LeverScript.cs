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

    public Transform waterBarTransform;
    private float holdTime = 0f;
    private float maxHoldDuration = 6f;
    private bool puzzleCompleteChecked = false;

    private int[] digits = new int[4];
    private int selectedIndex = 0;
    private bool isPuzzleActive = false;
    private PlayerMovement playerMovement;

    private bool isWaterLever = false;

    private bool playerInRange = false;

    void Start()
    {
        isWaterLever = gameObject.name == "Lever_C";
    }

    void Update()
    {
        if (!isPuzzleActive && playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            puzzleUI.SetActive(true);
            for (int i = 0; i < 4; i++) digits[i] = 0;
            selectedIndex = 0;
            UpdateDisplay();
            holdTime = 0f;
            puzzleCompleteChecked = false;
            if (waterBarTransform != null)
                waterBarTransform.localScale = new Vector3(1, 0, 1);
            isPuzzleActive = true;
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            if (playerMovement != null)
                playerMovement.enabled = false;
        }

        if (!isPuzzleActive) return;

        if (isWaterLever && !puzzleCompleteChecked && puzzleUI.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                holdTime += Time.deltaTime;
                float fillRatio = Mathf.Clamp01(holdTime / maxHoldDuration);
                if (waterBarTransform != null)
                    waterBarTransform.localScale = new Vector3(1, fillRatio, 1);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                puzzleCompleteChecked = true;
                if (Mathf.Abs(holdTime - maxHoldDuration) <= 0.2f)
                {
                    Debug.Log("Water fill success!");
                    CompletePuzzle();
                }
                else
                {
                    Debug.Log("Water fill failed!");
                    puzzleUI.SetActive(false);
                    if (playerMovement != null)
                        playerMovement.enabled = true;
                }
            }
        }

        if (!isWaterLever)
        {
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
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
