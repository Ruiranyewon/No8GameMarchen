using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;
    public GameObject[] players;
    private int currentIndex = 0;

    void Start()
    {
        ActivateOnly(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % players.Length;
            ActivateOnly(currentIndex);
        }
    }

    void ActivateOnly(int index)
    {
        Vector3 position = players[currentIndex].transform.position;

        for (int i = 0; i < players.Length; i++)
            players[i].SetActive(i == index);

        players[index].transform.position = position;

        if (cameraFollow != null)
            cameraFollow.target = players[index].transform;

        currentIndex = index;
    }
}
