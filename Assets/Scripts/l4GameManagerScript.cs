using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class L4GameManagerScript : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
    GameObject armorSupply;

    public Button continueButton;
    public Button quitButton;
    public GameObject introductionPanel;
    public Text StatusText;
    public Button restartButton;
    public Button nextButton;
    public GameObject blurMask;
    public Text BulletNumberText;
    public int bulletNumber = 10;
    public int supplyNumber = 8;
    public bool isStart = false;
    public bool armorAdded = false;

    void Start()
    {
        continueButton.onClick.AddListener(CloseIntroduction);
        quitButton.onClick.AddListener(ReturntoMainMenu);
        Time.timeScale = 0;

        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("Enemy");
        armorSupply = GameObject.FindWithTag("ArmorSupply");

        StatusText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        isStart = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowIntroduction();
        }

        if (armorSupply.GetComponent<L4ArmorSupplyScript>().isHit && !armorAdded)
        {
            armorAdded = true;
            bulletNumber += supplyNumber;
        }

        GameObject[] existingBullets = GameObject.FindGameObjectsWithTag("bullet");
        int count = existingBullets.Length;

        if ((player.GetComponent<L4CharacterScript>().isDead || (bulletNumber <= 0 && count <= 0)) && isStart)
        {
            // Debug.Log("Level failed!");
            isStart = false;
            ShowLevelStatus(false);
        }
        else if (enemy.GetComponent<L4EnemyScript>().isDead && isStart)
        {
            // Debug.Log("Level passed!");
            isStart = false;
            ShowLevelStatus(true);
        }

        BulletNumberText.text = "Fireballs: " + bulletNumber;
    }

    public void ShowIntroduction()
    {
        introductionPanel.SetActive(true);
        blurMask.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseIntroduction()
    {
        introductionPanel.SetActive(false);
        blurMask.SetActive(false);
        Thread.Sleep(100);
        Time.timeScale = 1;
    }
    
    // Show level status info and buttons
    void ShowLevelStatus(bool levelPassed)
    {
        if (levelPassed) {
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                StatusText.text = "Enemy Eliminated.\nLevel Passed!";
                nextButton.gameObject.SetActive(true);
                nextButton.onClick.AddListener(LoadNextLevel);
                StatusText.gameObject.SetActive(true);
                blurMask.SetActive(true);
            }
        }
        else
        {
            if (player.GetComponent<L4CharacterScript>().isDead)
                StatusText.text = "Player Died.\nLevel Failed!";
            else
                StatusText.text = "Out of Fireballs.\nLevel Failed!";
            restartButton.gameObject.SetActive(true);
            restartButton.onClick.AddListener(RestartLevel);
            StatusText.gameObject.SetActive(true);
            blurMask.SetActive(true);
        }
    }

    // Restart the level
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load the next level
    void LoadNextLevel()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ReturntoMainMenu()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Bgm"));
        SceneManager.LoadScene("HomeScene");
    }
}
