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

    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private string currentDirection = "";
    private Vector2 rushDirection;
    private float lastRushTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastRushTime = Time.time;

        // 바로 돌진 루프 시작
        StartCoroutine(RushRoutine());
    }

    void Update()
    {
        if (!isActivated)
        {
            if (FirewoodManager.firewoodCount >= requiredFirewoodCount)
            {
                isActivated = true;
            }
            else
            {
                return;
            }
        }

        if (player == null) return;

        if (!isRushing)
        {
            // 평소 이동
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
            SetAnimationDirection(dir);
        }
        else
        {
            // 돌진 중에는 rushDirection 고정
            transform.position += (Vector3)(rushDirection * rushSpeed * Time.deltaTime);
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
        while (true)
        {
            yield return new WaitUntil(() => isActivated);
            yield return new WaitForSeconds(rushCooldown);

            // 돌진 전 정지
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            rushDirection = dirToPlayer;

            yield return new WaitForSeconds(pauseBeforeRush);

            isRushing = true;
            yield return new WaitForSeconds(rushDuration);
            isRushing = false;
        }
    }
}
