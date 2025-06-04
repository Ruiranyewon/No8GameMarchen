using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GingerManAI : MonoBehaviour
{
    public Transform player;
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public LayerMask wallMask;

    private Rigidbody2D rb;
    private PathFinder pathfinder;
    private List<Vector2> path;
    private int pathIndex = 0;

    public float moveSpeed = 2f;
    public float repathTime = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(UpdatePath), 0f, repathTime);
    }

    void UpdatePath()
    {
        Vector2Int start = new Vector2Int(
            Mathf.FloorToInt(transform.position.x),
            Mathf.FloorToInt(transform.position.y)
        );

        Vector2Int end = new Vector2Int(
            Mathf.FloorToInt(player.position.x),
            Mathf.FloorToInt(player.position.y)
        );

        pathfinder = new PathFinder(bottomLeft, topRight, wallMask);
        path = pathfinder.FindPath(start, end);
        pathIndex = 0;

        if (path != null)
            Debug.Log("경로 계산됨: " + path.Count + "개 노드");
        else
            Debug.LogWarning("경로 없음: " + start + " → " + end);
    }

    private void FixedUpdate()
    {
        if (path == null || pathIndex >= path.Count)
            return;

        Vector2 targetPos = path[pathIndex];
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.05f)
            pathIndex++;
    }
}
