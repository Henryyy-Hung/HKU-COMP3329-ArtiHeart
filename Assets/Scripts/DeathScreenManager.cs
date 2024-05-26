using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreenPanel; // 将Panel拖拽到这个公共变量上
    public Button restartButton;
    public Button quitButton;
    

    private void Start()
    {
        deathScreenPanel.SetActive(false); // 初始时隐藏死亡信息面板
        restartButton.onClick.AddListener(RestartGame); // 为按钮添加点击事件监听
        quitButton.onClick.AddListener(QuitGame); // 为按钮添加点击事件监听
    }

    public void ShowDeathScreen()
    {
        deathScreenPanel.SetActive(true); // 显示死亡信息面板
    }

    void RestartGame()
    {
        //Time.timeScale = 1; // 恢复游戏
        //AudioListener.pause = false; // 恢复音效
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景

    }
    void QuitGame()
    {
       SceneManager.LoadScene("HomeScene");

    }
}
