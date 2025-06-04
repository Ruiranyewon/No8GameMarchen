using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public Vector2Int position;
    public Node parent;
    public int gCost; // 이동 거리
    public int hCost; // 목표까지 거리
    public int F => gCost + hCost;

    public Node(Vector2Int position)
    {
        this.position = position;
    }
}
