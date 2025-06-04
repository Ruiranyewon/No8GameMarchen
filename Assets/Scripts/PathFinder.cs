using System.Collections.Generic;
using UnityEngine;

public class CustomPathFinder
{
    private int width, height;
    private Vector2Int bottomLeft;
    private LayerMask wallMask;

    public CustomPathFinder(Vector2Int bottomLeft, Vector2Int topRight, LayerMask wallMask)
    {
        this.bottomLeft = bottomLeft;
        this.wallMask = wallMask;
        width = topRight.x - bottomLeft.x + 1;
        height = topRight.y - bottomLeft.y + 1;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        HashSet<Vector2Int> closedSet = new();
        PriorityQueue<Node> openSet = new();
        Dictionary<Vector2Int, Node> allNodes = new();

        Node startNode = new Node(start);
        startNode.gCost = 0;
        startNode.hCost = GetHeuristic(start, target);
        openSet.Enqueue(startNode, startNode.F);
        allNodes[start] = startNode;

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();
            if (current.position == target)
                return RetracePath(current);

            closedSet.Add(current.position);

            foreach (Vector2Int dir in Directions4)
            {
                Vector2Int neighborPos = current.position + dir;
                if (closedSet.Contains(neighborPos)) continue;

                if (IsWall(neighborPos)) continue;

                int tentativeG = current.gCost + 1;
                if (allNodes.TryGetValue(neighborPos, out Node neighbor))
                {
                    if (tentativeG < neighbor.gCost)
                    {
                        neighbor.gCost = tentativeG;
                        neighbor.parent = current;
                        openSet.Enqueue(neighbor, neighbor.F);
                    }
                }
                else
                {
                    neighbor = new Node(neighborPos);
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = GetHeuristic(neighborPos, target);
                    neighbor.parent = current;
                    allNodes[neighborPos] = neighbor;
                    openSet.Enqueue(neighbor, neighbor.F);
                }
            }
        }

        return null;
    }

    private List<Vector2Int> RetracePath(Node endNode)
    {
        List<Vector2Int> path = new();
        Node current = endNode;
        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    private int GetHeuristic(Vector2Int a, Vector2Int b) =>
        Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan °Å¸®

    private bool IsWall(Vector2Int pos)
    {
        Vector2 world = (Vector2)pos;
        return Physics2D.OverlapPoint(world, wallMask) != null;
    }

    private static readonly Vector2Int[] Directions4 =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };
}
