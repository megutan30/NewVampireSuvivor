using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider healthSlider; // HP�o�[��Slider�R���|�[�l���g
    public TextMeshProUGUI healthText; // HP��\������e�L�X�g

    private PlayerController playerController; // �v���C���[�R���g���[���[

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // ������HP��UI�ɔ��f����
        UpdateHealthUI();
    }

    void Update()
    {
        // HP���ω������ꍇ�AUI���X�V����
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // HP�o�[�̒l���v���C���[��HP�ɍ��킹��
        healthSlider.value = (float)playerController.currentHealth / playerController.maxHealth;

        // HP�e�L�X�g���X�V����
        healthText.text = "HP: " + playerController.currentHealth.ToString() + " / " + playerController.maxHealth.ToString();
    }
}
