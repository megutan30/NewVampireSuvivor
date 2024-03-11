using UnityEngine;

public class RecoveryItem : MonoBehaviour
{
    public int healAmount = 20; // �񕜗�
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
                attackController.Attack();
            }
            // �񕜃A�C�e����j�󂷂�
            Destroy(gameObject);
        }
    }
}
