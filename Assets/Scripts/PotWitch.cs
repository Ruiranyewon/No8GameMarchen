using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotWitch : MonoBehaviour
{
    public AudioClip collisionSound;  // 마녀가 닿았을 때 재생할 사운드
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 없으면 추가
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Witch"))
        {
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }
        }
    }
}
