using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinder : MonoBehaviour
{
    public Transform target; // 그레텔
    public float moveSpeed = 2f;
    public float pathUpdateInterval = 0.5f;

    private Grid grid;
    private Queue<Vector2> waypoints = new Queue<Vector2>();
    private Rigidbody2D rb;
    private Coroutine pathRoutine;

    void Start()
    {
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            Debug.LogError("[EnemyPathFinder] 타겟이 설정되지 않았습니다.");
            return;
        }

        pathRoutine = StartCoroutine(UpdatePathRoutine());
    }

    IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            if (target != null)
            {
                FindPath(transform.position, target.position);
            }
            yield return new WaitForSeconds(pathUpdateInterval);
        }
    }

    void Update()
    {
        if (waypoints.Count > 0)
        {
            Vector2 next = waypoints.Peek();
            Vector2 dir = (next - (Vector2)transform.position).normalized;
            rb.velocity = dir * moveSpeed;

            if (Vector2.Distance(transform.position, next) < 0.1f)
            {
                waypoints.Dequeue();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Debug.Log($"[FindPath] Start: {startNode.worldPosition}, walkable = {startNode.walkable}");
        Debug.Log($"[FindPath] Target: {targetNode.worldPosition}, walkable = {targetNode.walkable}");

        if (!startNode.walkable || !targetNode.walkable) return;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbours(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;

        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();

        waypoints.Clear();
        foreach (Node node in path)
        {
            waypoints.Enqueue(node.worldPosition);
        }

        Debug.Log("경로 생성 완료, 노드 수: " + waypoints.Count);
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);
        return dstX > dstY ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
    }
}
