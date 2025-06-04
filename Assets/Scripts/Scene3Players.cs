using UnityEngine;

public class Scene3Players : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;

    private int currentIndex = 0;
    private int maxAccessibleIndex = 2; // 0~2¹ø±îÁö¸¸ ÀüÈ¯ °¡´É (Çîµ¨ Á¦¿Ü)

    void Start()
    {
        ActivateOnly(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int nextIndex = (currentIndex + 1) % (maxAccessibleIndex + 1);
            SwitchPlayer(nextIndex);
        }
    }

    void SwitchPlayer(int newIndex)
    {
        Transform currentPlayer = transform.GetChild(currentIndex);
        Transform nextPlayer = transform.GetChild(newIndex);

        Vector3 lastPosition = currentPlayer.position;

        currentPlayer.gameObject.SetActive(false);
        nextPlayer.gameObject.SetActive(true);
        nextPlayer.position = lastPosition;

        if (cameraFollow != null)
            cameraFollow.target = nextPlayer;

        currentIndex = newIndex;
    }

    void ActivateOnly(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == index);

        if (cameraFollow != null)
            cameraFollow.target = transform.GetChild(index);

        currentIndex = index;
    }

    public void UnlockFourthPlayer()
    {
        if (maxAccessibleIndex < 3 && transform.childCount >= 4)
        {
            maxAccessibleIndex = 3;
            Debug.Log("Çîµ¨ Á¢±Ù °¡´ÉÇØÁü (Tab ¼øÈ¯ Æ÷ÇÔ)");
        }
    }

    public GameObject GetCurrentPlayer()
    {
        if (transform.childCount > 0)
            return transform.GetChild(currentIndex).gameObject;
        return null;
    }
}
