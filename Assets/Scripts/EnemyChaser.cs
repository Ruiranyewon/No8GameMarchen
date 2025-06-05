using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public float moveSpeed = 2f;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
            else
                return;
        }

        if (Time.timeScale == 0f)
            return;

        if (playerMovement != null && !PlayerMovement.canMove)
        {
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.identity;
    }
}
