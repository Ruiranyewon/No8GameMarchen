using UnityEngine;
using Pathfinding;

public class GingerTargetUpdater : MonoBehaviour
{
    public GameObject playersGroup; // "Players" 오브젝트를 Inspector에서 연결
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
