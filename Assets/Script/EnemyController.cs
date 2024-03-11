using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; // 敵の移動速度
    public int maxHealth = 50; // 敵の最大HP
    public int damage = 10; // 敵の攻撃ダメージ

    private int currentHealth; // 敵の現在のHP

    public GameObject recoveryItemPrefab; // 回復アイテムのプレハブ
    public float dropChance = 0.05f; // 回復アイテムを落とす確率

    Transform player;

    Rigidbody rb;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth; // 初期HPを最大HPに設定する
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // プレイヤーの方向に向かって移動する
        MoveEnemy();
    }

    void MoveEnemy()
    {
        transform.LookAt(player); // プレイヤーの方向を向く
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // プレイヤーの方向に向かって移動する
    }

    public void TakeDamage(int damage)
    {
        // ダメージを受けるとHPを減らす
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // HPが0以下になったら敵を破壊するなどの処理を行う
            Die();
        }
    }

    void Die()
    {
        // 敵が死亡したときの処理を実行する
        if (Random.value < dropChance)
        {
            DropRecoveryItem();
        }
        Destroy(gameObject);
    }

    void DropRecoveryItem()
    {
        // 回復アイテムを生成して落とす
        Vector3 pos =transform.position;
        pos.y += 0.5f;
        Instantiate(recoveryItemPrefab, pos, Quaternion.identity);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがプレイヤーであればダメージを与える
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            //Destroy(gameObject);
        }
    }
}
