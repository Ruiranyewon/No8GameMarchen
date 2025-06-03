using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMessagePopup : MonoBehaviour
{
    public GameObject messagePanel;

    void Start()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }
    }

    void Update()
    {
        
    }

    public void CloseMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
}
