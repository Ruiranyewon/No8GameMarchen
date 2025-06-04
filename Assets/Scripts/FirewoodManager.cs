using UnityEngine;

public class FirewoodManager : MonoBehaviour
{
    public static int firewoodCount = 0;

    public static void AddFirewood()
    {
        firewoodCount++;
        Debug.Log("Firewood collected! Total: " + firewoodCount);
    }
}
