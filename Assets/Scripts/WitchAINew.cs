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

    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private string currentDirection = "";
    private Vector2 rushDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(RushRoutine());
    }

    void Update()
    {
        if (!isActivated)
        {
            if (FirewoodManager.firewoodCount >= requiredFirewoodCount)
                isActivated = true;
            else
                return;
        }

        if (player == null) return;

        if (isPausing)
        {
            // 대기 상태: 아무것도 안 함
            return;
        }

        if (isRushing)
        {
            // 돌진 상태: 고정 방향
            transform.position += (Vector3)(rushDirection * rushSpeed * Time.deltaTime);
        }
        else
        {
            // 일반 추격
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

        while (true)
        {
            yield return new WaitForSeconds(rushCooldown);

            // 1. 추격 중단
            isPausing = true;

            // 2. 방향 고정
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            rushDirection = dirToPlayer;

            // 3. 잠깐 멈춤
            yield return new WaitForSeconds(pauseBeforeRush);

            // 4. 돌진
            isPausing = false;
            isRushing = true;

            yield return new WaitForSeconds(rushDuration);

            // 5. 다시 일반 추격
            isRushing = false;
        }
    }
}
