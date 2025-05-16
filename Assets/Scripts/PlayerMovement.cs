using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float runSpeed = 2f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // --- �� �Է� ó�� ---
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // �밢�� �̵� ����
        if (moveInput.sqrMagnitude > 1)
            moveInput = moveInput.normalized;

        // ����Ʈ�� �޸���
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed + runSpeed : speed;

        // --- �� �ִϸ����Ϳ� �� ���� ---
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);

            // ���� ����: �ϳ��� �ุ ����
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
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed + runSpeed : speed;
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }
}
