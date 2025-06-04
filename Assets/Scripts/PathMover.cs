using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMover : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public Vector2Int bottomLeft, topRight;
    public LayerMask wallMask;

    private CustomPathFinder pathFinder;
    private Queue<Vector2Int> pathQueue = new();
    private Vector2Int lastPlayerPos;

    private void Start()
    {
        pathFinder = new CustomPathFinder(bottomLeft, topRight, wallMask);
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f); // 플레이어 추적 주기
    }

    void Update()
    {
        if (pathQueue.Count > 0)
        {
            Vector2 next = (Vector2)pathQueue.Peek();
            Vector2 curr = (Vector2)transform.position;
            if (Vector2.Distance(curr, next) < 0.05f)
            {
                pathQueue.Dequeue();
            }
            else
            {
                Vector2 dir = (next - curr).normalized;
                transform.Translate(dir * moveSpeed * Time.deltaTime);
            }
        }
    }

    void UpdatePath()
    {
        Vector2Int start = Vector2Int.RoundToInt(transform.position);
        Vector2Int goal = Vector2Int.RoundToInt(target.position);

        if (goal != lastPlayerPos)
        {
            List<Vector2Int> newPath = pathFinder.FindPath(start, goal);
            if (newPath != null && newPath.Count > 0)
            {
                pathQueue = new Queue<Vector2Int>(newPath);
                lastPlayerPos = goal;
            }
        }
    }
}
