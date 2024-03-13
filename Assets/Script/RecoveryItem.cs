using UnityEngine;

public class RecoveryItem : MonoBehaviour
{
    public int healAmount = 20; // �񕜗�
    public float explosionForce = 100f; // ������΂��̗�
    public float explosionRadius = 5f; // ������΂��͈̔�

    AttackController attackController;

    private void Start()
    {
        attackController = GameObject.FindGameObjectWithTag("Attack").GetComponent<AttackController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // �Փ˂����I�u�W�F�N�g���v���C���[�������ꍇ
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // �v���C���[��HP���񕜂��A����𒴂��Ȃ��悤�ɂ���
                playerController.Heal(healAmount);
                //attackController.Attack();

                // ���͂̓G�𐁂���΂�
                ExplodeEnemies();
            }
            // �񕜃A�C�e����j�󂷂�
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
                    // ������΂��̗͂�������
                    enemyRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }
    }
}
