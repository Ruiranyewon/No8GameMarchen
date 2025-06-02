using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
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

        // --- �ִϸ��̼� ó�� ---
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
