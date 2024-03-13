using UnityEngine;

public class AttackController : MonoBehaviour
{
    public float baseRadius = 1f; // ��{�̍U���͈͂̔��a
    public float maxRadiusMultiplier = 2f; // �ő�̔��a�̔{��
    public float minRadiusMultiplier = 0.5f; // �ŏ��̔��a�̔{��
    public float attackInterval = 2f; // �U���̊Ԋu
    public int damage = 10; // �U�����̃_���[�W

    public bool switchAttackMethod =false;// �U���̎d�g�݂�؂�ւ�

    public Transform hpGaugeTransform; // HP�Q�[�W��Transform�R���|�[�l���g

    private PlayerController playerController; // �v���C���[�R���g���[���[
    private float timer; // �U���̊Ԋu���Ǘ�����^�C�}�[

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        attackInterval = playerController.minInterval;
        // �����̍U���͈͂�ݒ肷��
        SetAttackRadius();
    }

    void Update()
    {
        SetAttackRadius();
        // �^�C�}�[���X�V���A�U���̊Ԋu���Ǘ�����
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            // ���Ԋu���ƂɍU�����s��
            Attack();
            timer = 0f;
        }
    }

    void SetAttackRadius()
    {
        // �v���C���[��HP�ɉ����čU���͈͂̔��a��ݒ肷��
        float radiusMultiplier = Mathf.Lerp(minRadiusMultiplier, maxRadiusMultiplier, (float)playerController.currentHealth / playerController.maxHealth);
        transform.localScale = new Vector3(baseRadius * radiusMultiplier, baseRadius * radiusMultiplier, baseRadius * radiusMultiplier);
    }

    public void Attack()
    {
        if (!switchAttackMethod)
        {
            Vector3 inputDirection = playerController.GetInputDirection();

            // �v���C���[�̈ʒu
            Vector3 playerPosition = playerController.transform.position;

            // �U���͈͂̃I�u�W�F�N�g���v���C���[�̕����Ɉړ�����
            transform.position = playerPosition + inputDirection.normalized * (gameObject.transform.localScale.x / 2);

            // �U���͈͓��ɂ���G�Ƀ_���[�W��^����
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
            /*�߂��G*/
            // �v���C���[�̈ʒu
            Vector3 playerPosition = playerController.transform.position;

            // �v���C���[�ɍł��߂��G�̈ʒu���擾
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
                // �v���C���[�ɍł��߂��G�̕����x�N�g�����v�Z���A���K�������a������
                Vector3 direction = (nearestEnemy.transform.position - playerPosition).normalized;
                Vector3 pos = direction * (gameObject.transform.localScale.x / 2);

                // �U���͈͂̃I�u�W�F�N�g���v���C���[�̕����Ɉړ�����
                transform.position = playerPosition + pos;

                // HP�Q�[�W���U�������������iX����]90�x�j
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                hpGaugeTransform.rotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y - 90f, 0f);

                // �U���͈͓��ɂ���G�Ƀ_���[�W��^����
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