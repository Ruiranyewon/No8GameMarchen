using System.Collections;
using UnityEngine;
using Pathfinding;

public class SpecialEnemySpawner : MonoBehaviour
{
    public GameObject gnomePrefab;
    public GameObject gingerPrefab;
    public GameObject witchPrefab;

    public float spawnInterval = 3f;

    private PlayerMovement player;
    private bool witchSummoned = false;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomEnemy()
    {
        Vector3 pos = GetRandomWalkablePosition();
        if (pos == Vector3.zero) return;

        GameObject prefabToSpawn = Random.value < 0.5f ? gnomePrefab : gingerPrefab;
        GameObject enemy = Instantiate(prefabToSpawn, pos, Quaternion.identity);

        // 👇 공통: 추적 대상 지정
        SetTargetToPlayer(enemy);
    }

    IEnumerator SummonWitch()
    {
        witchSummoned = true;

        Vector3 spawnPos = GetRandomWalkablePosition();
        if (spawnPos == Vector3.zero)
        {
            witchSummoned = false;
            yield break;
        }

        GameObject witch = Instantiate(witchPrefab, spawnPos, Quaternion.identity);
        SetTargetToPlayer(witch);

        yield return new WaitForSeconds(3f);
        Destroy(witch);
        witchSummoned = false;
    }

    void SetTargetToPlayer(GameObject enemy)
    {
        if (player == null) return;

        AIDestinationSetter setter = enemy.GetComponent<AIDestinationSetter>();
        if (setter != null)
        {
            setter.target = player.transform;
        }
    }

    Vector3 GetRandomWalkablePosition()
    {
        GridGraph grid = AstarPath.active.data.gridGraph;

        for (int i = 0; i < 100; i++)
        {
            int x = Random.Range(0, grid.width);
            int y = Random.Range(0, grid.depth);
            var node = grid.GetNode(x, y);

            if (node != null && node.Walkable)
                return (Vector3)node.position;
        }

        Debug.LogWarning("[SpecialEnemySpawner] 유효한 위치를 찾지 못했습니다.");
        return Vector3.zero;
    }

    void Update()
    {
        if (player != null && player.IsStaminaZero() && !witchSummoned)
        {
            StartCoroutine(SummonWitch());
        }
    }
}
