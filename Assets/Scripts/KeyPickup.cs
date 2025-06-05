using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DoorTrigger door = FindObjectOfType<DoorTrigger>();
            if (door != null)
            {
                door.hasKey = true;
                Debug.Log("열쇠 획득: 문 열기 가능!");
            }

            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound);

            Destroy(gameObject);
        }
    }
}
