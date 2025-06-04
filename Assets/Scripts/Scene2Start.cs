using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Start : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Scene2Visited", 1);
        PlayerPrefs.Save();
    }

}
