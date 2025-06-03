using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    public GameObject keyPrefab;
    private int hitCount = 0;
    public int maxHits = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hitCount++;
            if (hitCount >= maxHits)
            {
                StartCoroutine(HandleDestruction());   
            }
        }
    }

    private void SpawnKey()
    {
        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
        }
    }

        private IEnumerator HandleDestruction()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnKey();
        Destroy(gameObject);
    }
}
