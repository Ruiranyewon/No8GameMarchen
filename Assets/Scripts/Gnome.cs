using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    public enum GnomeType { Normal, GingerMan, Boss }
    public GnomeType gnomeType = GnomeType.Normal;

    public GameObject keyPrefab;
    private int hitCount = 0;
    private int maxHits;

    private void Start()
    {
        switch (gnomeType)
        {
            case GnomeType.Normal:
                maxHits = 3; // fire bullets only
                break;
            case GnomeType.GingerMan:
                maxHits = 4; // water bullets only
                break;
            case GnomeType.Boss:
                maxHits = 10; // fire bullets only
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gnomeType == GnomeType.Normal || gnomeType == GnomeType.Boss)
        {
            if (other.CompareTag("FireBullet"))
            {
                RegisterHit();
                Destroy(other.gameObject);
            }
        }
        else if (gnomeType == GnomeType.GingerMan)
        {
            if (other.CompareTag("WaterBullet"))
            {
                RegisterHit();
                Destroy(other.gameObject);
            }
        }
    }

    private void RegisterHit()
    {
        hitCount++;
        if (hitCount >= maxHits)
        {
            StartCoroutine(HandleDestruction());
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
        gameObject.SetActive(false);
    }
}
