using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Start : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Scene1Visited", 1);
        PlayerPrefs.Save();
    }

}
