using UnityEngine;

public class RecoveryItem : MonoBehaviour
{
    public int healAmount = 20; // 回復量
    public float explosionForce = 100f; // 吹き飛ばしの力
    public float explosionRadius = 5f; // 吹き飛ばしの範囲

    AttackController attackController;

    private void Start()
    {
        attackController = GameObject.FindGameObjectWithTag("Attack").GetComponent<AttackController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトがプレイヤーだった場合
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // プレイヤーのHPを回復し、上限を超えないようにする
                playerController.Heal(healAmount);
                //attackController.Attack();

                // 周囲の敵を吹き飛ばす
                ExplodeEnemies();
            }
            // 回復アイテムを破壊する
            Destroy(gameObject);
        }
    }

    void ExplodeEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Rigidbody enemyRigidbody = collider.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    // 吹き飛ばしの力を加える
                    enemyRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }
    }
}
