using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // �v���C���[�̈ړ����x
    public int maxHealth = 100; // �v���C���[�̍ő�HP

    public int currentHealth; // �v���C���[�̌��݂�HP

    Rigidbody rb;
    Vector3 movement;
    AttackController[] attack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; // ����HP���ő�HP�ɐݒ肷��
    }

    void Update()
    {
        // �L�[�{�[�h�̓��͂��擾���A�ړ����������肷��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    void FixedUpdate()
    {
        // �v���C���[���ړ�����
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 direction)
    {
        // �v���C���[���ړ������Ɉړ�����
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        // �_���[�W���󂯂��HP�����炷
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // HP��0�ȉ��ɂȂ�����v���C���[��j�󂷂�Ȃǂ̏������s��
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // HP���ő�l�𒴂��Ȃ��悤�ɂ���
    }

    void Die()
    {
        // �v���C���[�����S�����Ƃ��̏��������s����
        Destroy(gameObject);
    }

    public Vector3 GetInputDirection()
    {
        // �v���C���[�̓��͕������擾����
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        return inputDirection;
    }
}