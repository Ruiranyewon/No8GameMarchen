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
        // --- ① 입력 처리 ---
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 대각선 이동 보정
        if (moveInput.sqrMagnitude > 1)
            moveInput = moveInput.normalized;

        // 쉬프트로 달리기
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed + runSpeed : speed;

        // --- ② 애니메이터에 값 전달 ---
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);

            // 방향 결정: 하나의 축만 저장
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
