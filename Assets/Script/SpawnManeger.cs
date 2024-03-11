using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManeger : MonoBehaviour
{
    public GameObject enemyPrefab; // �X�|�[������G�̃v���n�u
    public float spawnInterval = 2f; // �X�|�[���̊Ԋu
    public float spawnRange = 10f; 
    float timer;

    void Update()
    {
        // �^�C�}�[���X�V���A���Ԋu���ƂɓG���X�|�[������
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // ��ʂ̃����_���Ȉʒu�ɓG���X�|�[������
        //float spawnX = Random.Range(-spawnRange, spawnRange);
        //float spawnZ = Random.Range(-spawnRange, spawnRange);
        //Vector3 spawnPosition = new Vector3(spawnX, 0f, spawnZ);
        //Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // �����_���ȕ����𐶐�
        Vector3 randomDirection = new Vector3(Random.insideUnitSphere.x,0f, Random.insideUnitSphere.z).normalized;

        // �X�|�[���ʒu���v�Z
        Vector3 spawnPosition = transform.position + randomDirection * spawnRange;

        // �X�|�[������G�𐶐�
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
