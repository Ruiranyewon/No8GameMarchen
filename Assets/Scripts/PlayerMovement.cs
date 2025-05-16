using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float runSpeed = 2f;

    public float maxStamina = 5f;
    public float staminaRecoveryRate = 1f;
    public float staminaDrainRate = 2f;

    public float exhaustedDuration = 2f; // 탈진 지속 시간
    private float exhaustedTimer = 0f;
    private bool isExhausted = false;

    public Slider staminaBar;

    private float currentStamina;
    private bool isRunning = false;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;

        if (staminaBar != null)
            staminaBar.value = 1f;
    }

    private void Update()
    {
        // --- 입력 처리 ---
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.sqrMagnitude > 1)
            moveInput = moveInput.normalized;

        // 탈진 상태 체크
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

        // 달리기 조건: Shift + 이동 중 + 스태미나 있음 + 탈진 아님
        isRunning = Input.GetKey(KeyCode.LeftShift) && moveInput != Vector2.zero && currentStamina > 0 && !isExhausted;

        // 스태미나 처리
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

        // 스태미나 바 UI 업데이트
        if (staminaBar != null)
            staminaBar.value = currentStamina / maxStamina;

        // 현재 속도 계산
        float currentSpeed = isRunning ? speed + runSpeed : speed;

        // --- 애니메이션 처리 ---
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);

            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                animator.SetFloat("lastmoveX", Mathf.Sign(moveInput.x));
                animator.SetFloat("lastmoveY", 0);
            }
            else
            {
                animator.SetFloat("lastmoveX", 0);
                animator.SetFloat("lastmoveY", Mathf.Sign(moveInput.y));
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = isRunning ? speed + runSpeed : speed;
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }
}
