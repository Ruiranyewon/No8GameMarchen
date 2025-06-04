using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public Vector2Int position;
    public Node parent;
    public int gCost; // �̵� �Ÿ�
    public int hCost; // ��ǥ���� �Ÿ�
    public int F => gCost + hCost;

    public Node(Vector2Int position)
    {
        this.position = position;
    }
}
