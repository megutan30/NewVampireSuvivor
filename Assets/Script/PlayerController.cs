using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // �v���C���[�̈ړ����x
    public int maxHealth = 100; // �v���C���[�̍ő�HP
    public float maxInterval = 3.0f; // �ŏ��T�C�Y
    public float minInterval = 0.5f; // �ŏ��T�C�Y
    public float invincibleDuration = 1.5f; // ���G����
    public int currentHealth; // �v���C���[�̌��݂�HP

    private bool isInvincible = false; // �v���C���[�����G��Ԃ��ǂ����̃t���O
    private float invincibleTimer = 0f; // ���G���Ԃ̃^�C�}�[
    Rigidbody rb;
    Vector3 movement;
    AttackController attackController;

    void Start()
    {
        attackController = GameObject.FindGameObjectWithTag("Attack").GetComponent<AttackController>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; // ����HP���ő�HP�ɐݒ肷��
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
        if (isInvincible) return;
        
        // �_���[�W���󂯂��HP�����炷
        currentHealth -= damage;

        float newAttackInterval = Mathf.Lerp(maxInterval, minInterval, (float)currentHealth / maxHealth);
        attackController.attackInterval = newAttackInterval;
        //attackController.Attack();
        if (currentHealth <= 0)
        {
            // HP��0�ȉ��ɂȂ�����v���C���[��j�󂷂�Ȃǂ̏������s��
            Die();
        }
        isInvincible = true;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // HP���ő�l�𒴂��Ȃ��悤�ɂ���
        //attackController.Attack();
        float newAttackInterval = Mathf.Lerp(maxInterval, minInterval, (float)currentHealth / maxHealth);
        attackController.attackInterval = newAttackInterval;
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