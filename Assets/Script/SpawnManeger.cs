using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManeger : MonoBehaviour
{
    public GameObject enemyPrefab; // スポーンする敵のプレハブ
    public float spawnInterval = 2f; // スポーンの間隔
    public float spawnRange = 10f; 
    float timer;

    void Update()
    {
        // タイマーを更新し、一定間隔ごとに敵をスポーンする
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // 画面のランダムな位置に敵をスポーンする
        //float spawnX = Random.Range(-spawnRange, spawnRange);
        //float spawnZ = Random.Range(-spawnRange, spawnRange);
        //Vector3 spawnPosition = new Vector3(spawnX, 0f, spawnZ);
        //Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // ランダムな方向を生成
        Vector3 randomDirection = new Vector3(Random.insideUnitSphere.x,0f, Random.insideUnitSphere.z).normalized;

        // スポーン位置を計算
        Vector3 spawnPosition = transform.position + randomDirection * spawnRange;

        // スポーンする敵を生成
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
