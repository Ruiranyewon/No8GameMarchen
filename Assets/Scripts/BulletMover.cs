using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private float moveSpeed = 5f;

    public void SetDirectionAndSpeed(Vector2 direction, float speed)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        moveSpeed = speed;
    }

    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
