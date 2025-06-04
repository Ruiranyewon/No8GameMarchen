using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Scene3Foot : MonoBehaviour
{
    public AudioClip walkClip;
    public AudioClip runClip;
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f;

    private AudioSource audioSource;
    private float timer = 0f;
    private bool wasMoving = false;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (rb == null || animator == null) return;

        bool isMoving = animator.GetBool("isMoving");

        if (!PlayerMovement.canMove)
        {
            wasMoving = false;
            return;
        }

        if (isMoving)
        {
            if (!wasMoving) timer = 0f;

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                bool isRunning = PlayerMovement.canMove && Input.GetKey(KeyCode.LeftShift) && rb.velocity != Vector2.zero;
                audioSource.clip = isRunning ? runClip : walkClip;
                audioSource.Play();

                timer = isRunning ? runInterval : walkInterval;
            }
        }
        else
        {
            timer = 0f;
        }

        wasMoving = isMoving;
    }
}
