using System.Collections;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip1;
    public float fadeDuration = 2f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = bgmClip1;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    void Update()
    {
        if (audioSource == null || audioSource.clip == null)
            return;

        if (IsUIPanelActive())
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        else
        {
            if (!audioSource.isPlaying)
                audioSource.UnPause();
        }
    }

    private bool IsUIPanelActive()
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            if (!go.activeInHierarchy) continue;

            string lowerName = go.name.ToLower();
            if (lowerName.Contains("panel"))
                return true;
        }
        return false;
    }
}