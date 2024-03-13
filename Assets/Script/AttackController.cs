using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float baseRadius = 1f; // 基本の攻撃範囲の半径
    public float maxRadiusMultiplier = 2f; // 最大の半径の倍率
    public float minRadiusMultiplier = 0.5f; // 最小の半径の倍率
    public float attackInterval = 2f; // 攻撃の間隔
    public int damage = 10; // 攻撃時のダメージ

    public bool switchAttackMethod =false;// 攻撃の仕組みを切り替え

    public Transform hpGaugeTransform; // HPゲージのTransformコンポーネント

    private PlayerController playerController; // プレイヤーコントローラー
    private float timer; // 攻撃の間隔を管理するタイマー

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        attackInterval = playerController.minInterval;
        // 初期の攻撃範囲を設定する
        SetAttackRadius();
    }

    void Update()
    {
        SetAttackRadius();
        // タイマーを更新し、攻撃の間隔を管理する
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            // 一定間隔ごとに攻撃を行う
            Attack();
            timer = 0f;
        }
    }

    void SetAttackRadius()
    {
        // プレイヤーのHPに応じて攻撃範囲の半径を設定する
        float radiusMultiplier = Mathf.Lerp(minRadiusMultiplier, maxRadiusMultiplier, (float)playerController.currentHealth / playerController.maxHealth);
        transform.localScale = new Vector3(baseRadius * radiusMultiplier, baseRadius * radiusMultiplier, baseRadius * radiusMultiplier);
    }

    public void Attack()
    {
        if (!switchAttackMethod)
        {
            Vector3 inputDirection = playerController.GetInputDirection();

            // プレイヤーの位置
            Vector3 playerPosition = playerController.transform.position;

            // 攻撃範囲のオブジェクトをプレイヤーの方向に移動する
            transform.position = playerPosition + inputDirection.normalized * (gameObject.transform.localScale.x / 2);

            // 攻撃範囲内にある敵にダメージを与える
            Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<EnemyController>().TakeDamage(damage);
                }
            }
        }
        else
        {
            /*近い敵*/
            // プレイヤーの位置
            Vector3 playerPosition = playerController.transform.position;

            // プレイヤーに最も近い敵の位置を取得
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject nearestEnemy = null;
            float nearestDistance = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(playerPosition, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null)
            {
                // プレイヤーに最も近い敵の方向ベクトルを計算し、正規化し半径分離す
                Vector3 direction = (nearestEnemy.transform.position - playerPosition).normalized;
                Vector3 pos = direction * (gameObject.transform.localScale.x / 2);

                // 攻撃範囲のオブジェクトをプレイヤーの方向に移動する
                transform.position = playerPosition + pos;

                // HPゲージも攻撃方向を向く（X軸回転90度）
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                hpGaugeTransform.rotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y - 90f, 0f);

                // 攻撃範囲内にある敵にダメージを与える
                Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 2);
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        collider.GetComponent<EnemyController>().TakeDamage(damage);
                    }
                }
            }
        }
    }
}