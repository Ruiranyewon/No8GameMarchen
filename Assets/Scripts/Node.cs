using UnityEngine;

public class Node
{
    public bool isWall;
    public bool walkable;
    public Vector3 worldPosition;
    public int x, y;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    public Node(bool isWall, int x, int y)
    {
        this.isWall = isWall;
        this.x = x;
        this.y = y;
    }
}
