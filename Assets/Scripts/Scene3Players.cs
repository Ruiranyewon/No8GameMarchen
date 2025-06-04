using UnityEngine;

public class Scene3Players : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    public GameObject[] players;
    private int currentIndex = 0;

    void Start()
    {
        ActivateOnly(currentIndex, keepPosition: false); // 초기엔 자기 위치 유지
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int nextIndex = (currentIndex + 1) % players.Length;
            Vector3 switchPosition = players[currentIndex].transform.position;
            ActivateOnly(nextIndex, keepPosition: true, switchPosition);
        }
    }

    void ActivateOnly(int index, bool keepPosition, Vector3 position = default)
    {
        for (int i = 0; i < players.Length; i++)
            players[i].SetActive(i == index);

        if (keepPosition)
            players[index].transform.position = position;

        if (cameraFollow != null)
            cameraFollow.target = players[index].transform;

        currentIndex = index;
    }
}
