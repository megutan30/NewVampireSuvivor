using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public int maxHealth = 100; // プレイヤーの最大HP

    public int currentHealth; // プレイヤーの現在のHP

    Rigidbody rb;
    Vector3 movement;
    AttackController[] attack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; // 初期HPを最大HPに設定する
    }

    void Update()
    {
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
        // ダメージを受けるとHPを減らす
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // HPが0以下になったらプレイヤーを破壊するなどの処理を行う
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // HPが最大値を超えないようにする
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
}