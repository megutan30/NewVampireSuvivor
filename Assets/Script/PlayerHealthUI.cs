using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider healthSlider; // HPバーのSliderコンポーネント
    public TextMeshProUGUI healthText; // HPを表示するテキスト

    private PlayerController playerController; // プレイヤーコントローラー

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // 初期のHPをUIに反映する
        UpdateHealthUI();
    }

    void Update()
    {
        // HPが変化した場合、UIを更新する
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // HPバーの値をプレイヤーのHPに合わせる
        healthSlider.value = (float)playerController.currentHealth / playerController.maxHealth;

        // HPテキストを更新する
        healthText.text = "HP: " + playerController.currentHealth.ToString() + " / " + playerController.maxHealth.ToString();
    }
}
