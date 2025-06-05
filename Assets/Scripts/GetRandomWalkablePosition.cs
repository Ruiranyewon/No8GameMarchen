using UnityEngine;
using Pathfinding;

public class GetRandomWalkablePosition : MonoBehaviour
{
    public Vector3 GetPosition()
    {
        GridGraph grid = AstarPath.active.data.gridGraph;

        for (int i = 0; i < 100; i++)
        {
            int x = Random.Range(0, grid.width);
            int y = Random.Range(0, grid.depth);
            var node = grid.GetNode(x, y);

            if (node != null && node.Walkable)
                return (Vector3)node.position;
        }

        return Vector3.zero;
    }
}
