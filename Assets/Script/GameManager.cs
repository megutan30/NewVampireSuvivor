using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI resetText; // リセットテキスト
    private PlayerController playerController; // プレイヤーコントローラー

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        resetText.enabled = false; // リセットテキストを非表示にする
    }

    void Update()
    {
        // プレイヤーのHPが0になったらリセットテキストを表示する
        if (playerController.currentHealth <= 0)
        {
            resetText.enabled = true;
        }

        // Rボタンが押されたらリセットする
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    void ResetGame()
    {
        // 現在のシーンを再読み込みする
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
