using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static bool canMove = true;
    public float speed = 3f;
    public float runSpeed = 2f;

    public float maxStamina = 5f;
    public float staminaRecoveryRate = 1f;
    public float staminaDrainRate = 2f;

    public float exhaustedDuration = 2f; // Ż�� ���� �ð�
    private float exhaustedTimer = 0f;
    private bool isExhausted = false;

    public Slider staminaBar;

    private float currentStamina;
    private bool isRunning = false;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator currentAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("[PlayerMovement] Rigidbody2D component is missing on the 'Players' GameObject. Please add one in the Inspector.");
        }
        currentAnimator = GetComponentInChildren<Animator>();
        currentStamina = maxStamina;

        if (staminaBar != null)
            staminaBar.value = 1f;
    }

    private void Update()
    {
        if (!canMove) return;
        // --- �Է� ó�� ---
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0 && inputY != 0)
            inputY = 0; // Prevent diagonal movement

        moveInput = new Vector2(inputX, inputY);

        if (moveInput.sqrMagnitude > 1)
            moveInput = moveInput.normalized;

        // Ż�� ���� üũ
        if (currentStamina <= 0 && !isExhausted)
        {
            isExhausted = true;
            exhaustedTimer = exhaustedDuration;
        }

        if (isExhausted)
        {
            exhaustedTimer -= Time.deltaTime;
            if (exhaustedTimer <= 0f)
            {
                isExhausted = false;
            }
        }

        // �޸��� ����: Shift + �̵� �� + ���¹̳� ���� + Ż�� �ƴ�
        isRunning = Input.GetKey(KeyCode.LeftShift) && moveInput != Vector2.zero && currentStamina > 0 && !isExhausted;

        // ���¹̳� ó��
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        // ���¹̳� �� UI ������Ʈ
        if (staminaBar != null)
            staminaBar.value = currentStamina / maxStamina;

        // ���� �ӵ� ���
        float currentSpeed = isRunning ? speed + runSpeed : speed;

        // --- 애니메이션 처리 ---
        if (currentAnimator == null || !currentAnimator.gameObject.activeInHierarchy)
        {
            currentAnimator = GetComponentInChildren<Animator>();
        }

        if (currentAnimator != null)
        {
            currentAnimator.SetFloat("moveX", moveInput.x);
            currentAnimator.SetFloat("moveY", moveInput.y);

            if (moveInput != Vector2.zero)
            {
                currentAnimator.SetBool("isMoving", true);

                if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                {
                    currentAnimator.SetFloat("lastmoveX", Mathf.Sign(moveInput.x));
                    currentAnimator.SetFloat("lastmoveY", 0);
                }
                else
                {
                    currentAnimator.SetFloat("lastmoveX", 0);
                    currentAnimator.SetFloat("lastmoveY", Mathf.Sign(moveInput.y));
                }
            }
            else
            {
                currentAnimator.SetBool("isMoving", false);
            }
        }
        //Debug.Log($"[PlayerMovement] moveInput: {moveInput}, isRunning: {isRunning}, currentStamina: {currentStamina:F2}");
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        if (rb == null) return;
        float currentSpeed = isRunning ? speed + runSpeed : speed;
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.identity;
    }
        public static bool CanSwitchTo(string characterName)
    {
        switch (characterName)
        {
            case "Marin":
                GameObject marinTrapped = GameObject.Find("MarinTrapped");
                return marinTrapped == null || !marinTrapped.activeInHierarchy;

            case "Chili":
                GameObject chiliTrapped = GameObject.Find("ChiliTrapped");
                return chiliTrapped == null || !chiliTrapped.activeInHierarchy;

            default:
                return true;
        }
    }
}
