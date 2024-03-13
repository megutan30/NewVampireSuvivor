using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public int maxHealth = 100; // プレイヤーの最大HP
    public float maxInterval = 3.0f; // 最小サイズ
    public float minInterval = 0.5f; // 最小サイズ
    public float invincibleDuration = 1.5f; // 無敵時間
    public int currentHealth; // プレイヤーの現在のHP

    private bool isInvincible = false; // プレイヤーが無敵状態かどうかのフラグ
    private float invincibleTimer = 0f; // 無敵時間のタイマー
    Rigidbody rb;
    Vector3 movement;
    AttackController attackController;

    void Start()
    {
        attackController = GameObject.FindGameObjectWithTag("Attack").GetComponent<AttackController>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; // 初期HPを最大HPに設定する
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= invincibleDuration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
            }
        }
        // キーボードの入力を取得し、移動方向を決定する
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    void FixedUpdate()
    {
        // プレイヤーを移動する
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 direction)
    {
        // プレイヤーを移動方向に移動する
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        
        // ダメージを受けるとHPを減らす
        currentHealth -= damage;

        float newAttackInterval = Mathf.Lerp(maxInterval, minInterval, (float)currentHealth / maxHealth);
        attackController.attackInterval = newAttackInterval;
        //attackController.Attack();
        if (currentHealth <= 0)
        {
            // HPが0以下になったらプレイヤーを破壊するなどの処理を行う
            Die();
        }
        isInvincible = true;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // HPが最大値を超えないようにする
        //attackController.Attack();
        float newAttackInterval = Mathf.Lerp(maxInterval, minInterval, (float)currentHealth / maxHealth);
        attackController.attackInterval = newAttackInterval;
    }

    void Die()
    {
        // プレイヤーが死亡したときの処理を実行する
        Destroy(gameObject);
    }

    public Vector3 GetInputDirection()
    {
        // プレイヤーの入力方向を取得する
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        return inputDirection;
    }

    public Vector3 GetNearestEnemyDirection()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

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
            Vector3 direction = (nearestEnemy.transform.position - playerPosition).normalized;
            return direction;
        }
        else
        {
            return Vector3.zero;
        }
    }
}