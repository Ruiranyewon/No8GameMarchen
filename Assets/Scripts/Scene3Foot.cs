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
        if (!PlayerMovement.canMove) return;
        if (rb == null || animator == null) return;

        bool isMoving = animator.GetBool("isMoving");

        if (isMoving)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                bool isRunning = Input.GetKey(KeyCode.LeftShift); // 외부 참조 없이 키입력으로 판별
                audioSource.clip = isRunning ? runClip : walkClip;
                audioSource.Play();

                timer = isRunning ? runInterval : walkInterval;
            }
        }
        else
        {
            timer = 0f;
        }
    }
}
