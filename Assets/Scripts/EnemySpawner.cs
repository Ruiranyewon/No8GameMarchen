using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject gingerbreadPrefab;
    public GameObject sugarGnomePrefab;
    public GameObject bossGnomePrefab;

    public float spawnInterval = 5f;
    public int totalEnemiesToSpawn = 20;
    public Vector2 spawnAreaSize = new Vector2(600f, 400f);

    private int enemiesSpawned = 0;
    private float timer = 0f;
    private float elapsedTime = 0f;

    void Update()
    {
        if (enemiesSpawned > totalEnemiesToSpawn)
            return;

        timer += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            if (enemiesSpawned < totalEnemiesToSpawn)
            {
                GameObject toSpawn;

                if (enemiesSpawned < 3)
                {
                    toSpawn = gingerbreadPrefab;
                }
                else if (elapsedTime >= 20f)
                {
                    toSpawn = Random.value < 0.5f ? gingerbreadPrefab : sugarGnomePrefab;
                }
                else
                {
                    toSpawn = gingerbreadPrefab;
                }

                Instantiate(toSpawn, GetRandomPosition(), Quaternion.identity);
                enemiesSpawned++;
            }
            else if (enemiesSpawned == totalEnemiesToSpawn)
            {
                Instantiate(bossGnomePrefab, GetRandomPosition(), Quaternion.identity);
                enemiesSpawned++;
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
