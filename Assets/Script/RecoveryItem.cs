using UnityEngine;

public class RecoveryItem : MonoBehaviour
{
    public int healAmount = 20; // 回復量
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
                attackController.Attack();
            }
            // 回復アイテムを破壊する
            Destroy(gameObject);
        }
    }
}
