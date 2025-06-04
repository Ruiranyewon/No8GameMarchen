using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost { get { return gCost + hCost; } }

    public Node(bool _walkable, Vector3 _worldPos, int _x, int _y)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _x;
        gridY = _y;
    }
}
