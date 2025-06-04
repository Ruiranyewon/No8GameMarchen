using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3Start : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Scene3Visited", 1);
        PlayerPrefs.Save();
    }

}
