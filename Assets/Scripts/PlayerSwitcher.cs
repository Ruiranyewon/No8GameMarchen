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
            int nextIndex = (currentIndex + 1) % players.Length;
            string characterName = players[nextIndex].name;

            if (PlayerMovement.CanSwitchTo(characterName))
            {
                currentIndex = nextIndex;
                ActivateOnly(currentIndex);
            }
            else
            {
                Debug.Log($"[PlayerSwitcher] {characterName}은(는) 아직 전환할 수 없습니다.");
            }
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
