using UnityEngine;
using Pathfinding;

public class GingerTargetUpdater : MonoBehaviour
{
    public GameObject playersGroup; // "Players" ������Ʈ�� Inspector���� ����
    private AIDestinationSetter setter;

    void Start()
    {
        setter = GetComponent<AIDestinationSetter>();
    }

    void Update()
    {
        foreach (Transform child in playersGroup.transform)
        {
            if (child.gameObject.activeSelf)
            {
                if (setter.target != child)
                    setter.target = child;
                break;
            }
        }
    }
}
