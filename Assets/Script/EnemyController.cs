using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; // 敵の移動速度
    public int maxHealth = 50; // 敵の最大HP
    public int damage = 10; // 敵の攻撃ダメージ
    public float maxSize = 2f; // 最大サイズ
    public float minSize = 0.5f; // 最小サイズ
    public float damageDisplayDuration = 1f; // ダメージ表示の持続時間
    public GameObject damageTextPrefab; // ダメージ表示用テキストのプレハブ
    public GameObject recoveryItemPrefab; // 回復アイテムのプレハブ
    public float dropChance = 0.05f; // 回復アイテムを落とす確率
    public float reciveAttackCoolTime = 0.1f;

    private float coolTime= 0f;
    private int currentHealth; // 敵の現在のHP

    Transform player;
    Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth; // 初期HPを最大HPに設定する
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // プレイヤーの方向に向かって移動する
        MoveEnemy();
    }

    void MoveEnemy()
    {
        transform.LookAt(player); // プレイヤーの方向を向く
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // プレイヤーの方向に向かって移動する
    }

    public void TakeDamage(int damage)
    {
        // ダメージを受けるとHPを減らす
        currentHealth -= damage;

        // HPに応じてサイズを変更する
        float newSize = Mathf.Lerp(minSize, maxSize, (float)currentHealth / maxHealth);
        transform.localScale = new Vector3(newSize, newSize, newSize);

        // ダメージテキストを表示する
        ShowDamageText(damage);

        if (currentHealth <= 0)
        {
            // HPが0以下になったら敵を破壊するなどの処理を行う
            Die();
        }
    }

    void ShowDamageText(int damage)
    {
        GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageTextObject.transform.Rotate(new Vector3(90, 0, 0)); // X軸を中心に90度回転
        damageTextObject.GetComponent<TextMesh>().text = damage.ToString();
        Destroy(damageTextObject, damageDisplayDuration);
    }

    void Die()
    {
        // 敵が死亡したときの処理を実行する
        if (Random.value < dropChance)
        {
            DropRecoveryItem();
        }
        // オブジェクトを非アクティブにする代わりに、一定時間後に破棄する
        gameObject.SetActive(false);
        Destroy(gameObject, damageDisplayDuration);
    }

    void DropRecoveryItem()
    {
        // 回復アイテムを生成して落とす
        Vector3 pos = transform.position;
        pos.y += 0.5f;
        Instantiate(recoveryItemPrefab, pos, Quaternion.identity);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがプレイヤーであればダメージを与える
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
        // 衝突したオブジェクトがプレイヤーであればダメージを与える
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            coolTime = 0f;
        }
    }
}
