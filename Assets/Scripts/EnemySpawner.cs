using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject gingerbreadPrefab;
    public GameObject sugarGnomePrefab;
    public GameObject bossGnomePrefab;
    public GameObject chiliTrapped;
    private bool chiliRescued = false;
    private bool postRescueSpawned = false;

    public float spawnInterval = 5f;
    public Vector2 spawnAreaSize = new Vector2(600f, 400f);

    private float timer = 0f;

    void Update()
    {
        if (chiliTrapped != null && !chiliRescued && !chiliTrapped.activeInHierarchy)
        {
            chiliRescued = true;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            if (!chiliRescued)
            {
                // Before chili is rescued, spawn only gingerbread
                Instantiate(gingerbreadPrefab, GetRandomPosition(), Quaternion.identity);
            }
            else if (!postRescueSpawned)
            {
                // After chili is rescued, spawn 2 sugar gnomes and 1 boss gnome
                Instantiate(sugarGnomePrefab, GetRandomPosition(), Quaternion.identity);
                Instantiate(sugarGnomePrefab, GetRandomPosition(), Quaternion.identity);
                Instantiate(bossGnomePrefab, GetRandomPosition(), Quaternion.identity);
                postRescueSpawned = true;
            }
            else
            {
                // After post-rescue spawn, spawn gingerbread or sugar gnome randomly (excluding boss)
                GameObject toSpawn = Random.value < 0.5f ? gingerbreadPrefab : sugarGnomePrefab;
                Instantiate(toSpawn, GetRandomPosition(), Quaternion.identity);
            }
        }
    }

    Vector2 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float y = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        return new Vector2(x, y);
    }
}
