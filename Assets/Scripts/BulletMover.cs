using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private float moveSpeed = 50f;

    public void SetDirectionAndSpeed(Vector2 direction, float speed)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        moveSpeed = speed;
    }

    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
    }
}
