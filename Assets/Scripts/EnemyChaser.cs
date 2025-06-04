using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
            else
                return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        Debug.Log("Chasing direction: " + direction);
        movement = direction;
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        Debug.Log("Attempting to move to: " + newPosition);

        // Check direct path
        RaycastHit2D hit = Physics2D.Raycast(rb.position, movement, moveSpeed * Time.fixedDeltaTime);
        if (hit.collider == null || hit.collider.isTrigger)
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            Debug.Log("Blocked by: " + hit.collider.name);
            // Try perpendicular movement
            Vector2 perpMove = Mathf.Abs(movement.x) > Mathf.Abs(movement.y)
                ? new Vector2(0, movement.y).normalized
                : new Vector2(movement.x, 0).normalized;

            Vector2 altPosition = rb.position + perpMove * moveSpeed * Time.fixedDeltaTime;
            RaycastHit2D altHit = Physics2D.Raycast(rb.position, perpMove, moveSpeed * Time.fixedDeltaTime);
            if (altHit.collider == null || altHit.collider.isTrigger)
            {
                rb.MovePosition(altPosition);
            }
        }
    }
}
