using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; // �G�̈ړ����x
    public int maxHealth = 50; // �G�̍ő�HP
    public int damage = 10; // �G�̍U���_���[�W
    public float maxSize = 2f; // �ő�T�C�Y
    public float minSize = 0.5f; // �ŏ��T�C�Y
    public float damageDisplayDuration = 1f; // �_���[�W�\���̎�������
    public GameObject damageTextPrefab; // �_���[�W�\���p�e�L�X�g�̃v���n�u
    public GameObject recoveryItemPrefab; // �񕜃A�C�e���̃v���n�u
    public float dropChance = 0.05f; // �񕜃A�C�e���𗎂Ƃ��m��
    public float reciveAttackCoolTime = 0.1f;

    private float coolTime= 0f;
    private int currentHealth; // �G�̌��݂�HP

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

        // �_���[�W�e�L�X�g��\������
        ShowDamageText(damage);

        if (currentHealth <= 0)
        {
            // HP��0�ȉ��ɂȂ�����G��j�󂷂�Ȃǂ̏������s��
            Die();
        }
    }

    void ShowDamageText(int damage)
    {
        GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageTextObject.transform.Rotate(new Vector3(90, 0, 0)); // X���𒆐S��90�x��]
        damageTextObject.GetComponent<TextMesh>().text = damage.ToString();
        Destroy(damageTextObject, damageDisplayDuration);
    }

    void Die()
    {
        // �G�����S�����Ƃ��̏��������s����
        if (Random.value < dropChance)
        {
            DropRecoveryItem();
        }
        // �I�u�W�F�N�g���A�N�e�B�u�ɂ������ɁA��莞�Ԍ�ɔj������
        gameObject.SetActive(false);
        Destroy(gameObject, damageDisplayDuration);
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
    void OnCollisionStay(Collision collision)
    {
        coolTime += Time.deltaTime;
        Debug.Log(coolTime);
        if (coolTime < reciveAttackCoolTime) return;
        // �Փ˂����I�u�W�F�N�g���v���C���[�ł���΃_���[�W��^����
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            coolTime = 0f;
        }
    }
}
