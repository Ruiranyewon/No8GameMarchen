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
    private bool isStopped = false;

    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private string currentDirection = "";
    private Vector2 rushDirection;

    private float originalMoveSpeed;
    private float originalRushSpeed;

    public AudioClip rushSound;
    public AudioClip bgmOnFirstAppear; //  마녀 등장 시 한 번만 재생할 음악
    private bool bgmPlayed = false;    // 중복 방지용 플래그

    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalMoveSpeed = moveSpeed;
        originalRushSpeed = rushSpeed;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(RushRoutine());
    }

    void Update()
    {
        if (isStopped) return;

        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayer != null)
            player = currentPlayer.transform;

        if (!isActivated)
        {
            if (FirewoodManager.firewoodCount >= requiredFirewoodCount)
            {
                isActivated = true;

                // 마녀 등장 음악 한 번만 재생
                if (!bgmPlayed && bgmOnFirstAppear != null)
                {
                    audioSource.clip = bgmOnFirstAppear;
                    audioSource.loop = true;
                    audioSource.Play();
                    bgmPlayed = true;
                }
            }
            else
                return;
        }

        if (player == null || isPausing) return;

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

            if (rushSound != null && audioSource != null)
                audioSource.PlayOneShot(rushSound);

            yield return new WaitForSeconds(rushDuration);

            isRushing = false;
        }
    }

    public void StopMovement()
    {
        isStopped = true;
        isRushing = false;
        isPausing = false;
    }

    public void ResumeMovement()
    {
        isStopped = false;
        moveSpeed = originalMoveSpeed;
        rushSpeed = originalRushSpeed;
        StartCoroutine(RushRoutine());
    }
}
