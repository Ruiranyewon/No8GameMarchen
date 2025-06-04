using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private Node[,] grid;
    private int width, height;
    private Vector2Int bottomLeft;
    private LayerMask wallMask;

    public PathFinder(Vector2Int bottomLeft, Vector2Int topRight, LayerMask wallMask)
    {
        this.bottomLeft = bottomLeft;
        this.wallMask = wallMask;

        width = topRight.x - bottomLeft.x + 1;
        height = topRight.y - bottomLeft.y + 1;

        grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = new Vector2(x + bottomLeft.x + 0.5f, y + bottomLeft.y + 0.5f); // 중심 보정
                bool isWall = Physics2D.OverlapCircle(worldPos, 0.4f, wallMask);
                grid[x, y] = new Node(isWall, x + bottomLeft.x, y + bottomLeft.y);
            }
        }
    }

    public List<Vector2> FindPath(Vector2Int start, Vector2Int target)
    {
        int sx = start.x - bottomLeft.x;
        int sy = start.y - bottomLeft.y;
        int tx = target.x - bottomLeft.x;
        int ty = target.y - bottomLeft.y;

        if (sx < 0 || sy < 0 || tx < 0 || ty < 0 || sx >= width || sy >= height || tx >= width || ty >= height)
        {
            Debug.LogWarning($"시작 또는 목표 위치가 범위를 벗어났습니다: start={start}, target={target}");
            return null;
        }

        Node startNode = grid[sx, sy];
        Node targetNode = grid[tx, ty];

        List<Node> open = new List<Node> { startNode };
        HashSet<Node> closed = new HashSet<Node>();

        while (open.Count > 0)
        {
            Node current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].fCost < current.fCost || (open[i].fCost == current.fCost && open[i].hCost < current.hCost))
                    current = open[i];
            }

            open.Remove(current);
            closed.Add(current);

            if (current == targetNode)
            {
                List<Vector2> path = new List<Vector2>();
                Node temp = targetNode;
                while (temp != startNode)
                {
                    path.Add(new Vector2(temp.x + 0.5f, temp.y + 0.5f)); // 타일 중심으로 이동
                    temp = temp.parent;
                }
                path.Reverse();
                return path;
            }

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (neighbor.isWall || closed.Contains(neighbor)) continue;

                int moveCost = current.gCost + GetDistance(current, neighbor);
                if (moveCost < neighbor.gCost || !open.Contains(neighbor))
                {
                    neighbor.gCost = moveCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = current;

                    if (!open.Contains(neighbor))
                        open.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] dirs = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (Vector2Int dir in dirs)
        {
            int checkX = node.x + dir.x - bottomLeft.x;
            int checkY = node.y + dir.y - bottomLeft.y;
            if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                neighbors.Add(grid[checkX, checkY]);
        }

        return neighbors;
    }

    private int GetDistance(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
