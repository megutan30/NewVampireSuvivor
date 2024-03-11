using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI resetText; // ���Z�b�g�e�L�X�g
    private PlayerController playerController; // �v���C���[�R���g���[���[

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        resetText.enabled = false; // ���Z�b�g�e�L�X�g���\���ɂ���
    }

    void Update()
    {
        // �v���C���[��HP��0�ɂȂ����烊�Z�b�g�e�L�X�g��\������
        if (playerController.currentHealth <= 0)
        {
            resetText.enabled = true;
        }

        // R�{�^���������ꂽ�烊�Z�b�g����
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    void ResetGame()
    {
        // ���݂̃V�[�����ēǂݍ��݂���
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
