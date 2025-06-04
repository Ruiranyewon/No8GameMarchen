using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotWitch : MonoBehaviour
{
    public AudioClip collisionSound;  // ���డ ����� �� ����� ����
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // ������ �߰�
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
