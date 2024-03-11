using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; // �G�̈ړ����x
    public int maxHealth = 50; // �G�̍ő�HP
    public int damage = 10; // �G�̍U���_���[�W
    public float maxSize = 2f; // �ő�T�C�Y
    public float minSize = 0.1f; // �ŏ��T�C�Y

    private int currentHealth; // �G�̌��݂�HP

    public GameObject recoveryItemPrefab; // �񕜃A�C�e���̃v���n�u
    public float dropChance = 0.05f; // �񕜃A�C�e���𗎂Ƃ��m��

    Transform player;
    Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth; // ����HP���ő�HP�ɐݒ肷��
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // �v���C���[�̕����Ɍ������Ĉړ�����
        MoveEnemy();
    }

    void MoveEnemy()
    {
        transform.LookAt(player); // �v���C���[�̕���������
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // �v���C���[�̕����Ɍ������Ĉړ�����
    }

    public void TakeDamage(int damage)
    {
        // �_���[�W���󂯂��HP�����炷
        currentHealth -= damage;

        // HP�ɉ����ăT�C�Y��ύX����
        float newSize = Mathf.Lerp(minSize, maxSize, (float)currentHealth / maxHealth);
        transform.localScale = new Vector3(newSize, newSize, newSize);

        if (currentHealth <= 0)
        {
            // HP��0�ȉ��ɂȂ�����G��j�󂷂�Ȃǂ̏������s��
            Die();
        }
    }

    void Die()
    {
        // �G�����S�����Ƃ��̏��������s����
        if (Random.value < dropChance)
        {
            DropRecoveryItem();
        }
        Destroy(gameObject);
    }

    void DropRecoveryItem()
    {
        // �񕜃A�C�e���𐶐����ė��Ƃ�
        Vector3 pos = transform.position;
        pos.y += 0.5f;
        Instantiate(recoveryItemPrefab, pos, Quaternion.identity);
    }

    void OnCollisionEnter(Collision collision)
    {
        // �Փ˂����I�u�W�F�N�g���v���C���[�ł���΃_���[�W��^����
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
