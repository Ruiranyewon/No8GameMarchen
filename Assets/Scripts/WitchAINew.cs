using UnityEngine;
using System.Collections;

public class WitchAINew : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rushSpeed = 6f;
    public float rushDuration = 0.8f;
    public float pauseBeforeRush = 0.5f;
    public float rushCooldown = 5f;

    public int requiredFirewoodCount = 1;

    private bool isActivated = false;
    private bool isRushing = false;
    private bool isPausing = false;
    private bool isStopped = false; //  외부에서 완전 정지 시킬 때 사용

    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private string currentDirection = "";
    private Vector2 rushDirection;

    private float originalMoveSpeed;
    private float originalRushSpeed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalMoveSpeed = moveSpeed;
        originalRushSpeed = rushSpeed;

        StartCoroutine(RushRoutine());
    }

    void Update()
    {
        if (isStopped) return;

        // 현재 활성화된 플레이어 계속 추적
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayer != null)
            player = currentPlayer.transform;

        if (!isActivated)
        {
            if (FirewoodManager.firewoodCount >= requiredFirewoodCount)
                isActivated = true;
            else
                return;
        }

        if (player == null) return;

        if (isPausing) return;

        if (isRushing)
        {
            transform.position += (Vector3)(rushDirection * rushSpeed * Time.deltaTime);
        }
        else
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
            SetAnimationDirection(dir);
        }
    }

    void SetAnimationDirection(Vector2 dir)
    {
        string newDirection;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            newDirection = "Side";
            spriteRenderer.flipX = dir.x > 0;
        }
        else
        {
            newDirection = dir.y > 0 ? "Back" : "Front";
        }

        if (newDirection != currentDirection)
        {
            anim.SetTrigger(newDirection);
            currentDirection = newDirection;
        }
    }

    IEnumerator RushRoutine()
    {
        yield return new WaitUntil(() => isActivated);

        while (!isStopped)
        {
            yield return new WaitForSeconds(rushCooldown);

            if (player == null) yield break;

            isPausing = true;

            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            rushDirection = dirToPlayer;

            yield return new WaitForSeconds(pauseBeforeRush);

            isPausing = false;
            isRushing = true;

            yield return new WaitForSeconds(rushDuration);

            isRushing = false;
        }
    }

    // 외부에서 마녀를 멈추고 싶을 때 호출
    public void StopMovement()
    {
        isStopped = true;
        isRushing = false;
        isPausing = false;
    }

    // 외부에서 다시 살리고 싶으면 이 함수로!
    public void ResumeMovement()
    {
        isStopped = false;
        moveSpeed = originalMoveSpeed;
        rushSpeed = originalRushSpeed;
        StartCoroutine(RushRoutine());
    }
}
