using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float pathUpdateInterval = 0.5f;

    private List<Vector3> path = new List<Vector3>();
    private int currentWaypoint = 0;
    private Coroutine pathCoroutine;

    private Grid grid;

    void Start()
    {
        grid = GameObject.Find("GridManager").GetComponent<Grid>();
        pathCoroutine = StartCoroutine(UpdatePathRoutine());
    }

    void Update()
    {
        if (path.Count == 0 || currentWaypoint >= path.Count) return;

        Vector3 targetPos = path[currentWaypoint];
        Vector3 dir = (targetPos - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentWaypoint++;
        }
    }

    IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            FindPathToTarget();
            yield return new WaitForSeconds(pathUpdateInterval);
        }
    }

    void FindPathToTarget()
    {
        Node startNode = grid.NodeFromWorldPoint(transform.position);
        Node targetNode = grid.NodeFromWorldPoint(target.position);

        if (!startNode.walkable || !targetNode.walkable) return;

        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            Node current = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost || (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost))
                    current = openSet[i];
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbours(current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                int newCost = current.gCost + GetDistance(current, neighbor);
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> newPath = new List<Vector3>();
        Node current = endNode;

        while (current != startNode)
        {
            newPath.Add(current.worldPosition);
            current = current.parent;
        }

        newPath.Reverse();
        path = newPath;
        currentWaypoint = 0;
    }

    int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        return (dx > dy) ? 14 * dy + 10 * (dx - dy) : 14 * dx + 10 * (dy - dx);
    }
}
