using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuGUI; // 指向主菜单UI的引用
    public bool activated = true;
    public bool isFirstTime = true;

    public TextMeshProUGUI startButtonText;

    private void Start()
    {
        activated = true;
        mainMenuGUI.SetActive(true);

        int inGame = PlayerPrefs.GetInt("inGame");

        if (inGame == 1)
        {
            mainMenuGUI.SetActive(false);
            activated = false;
            isFirstTime = false;
            EventSystem.current.SetSelectedGameObject(null);
            startButtonText.text = "Continue";
            Time.timeScale = 1;
        }

        PlayerPrefs.SetInt("inGame", 0);

    }

    void Update()
    {
        DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();

        if (! dialogueSystem.isChatting && Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenuGUI.SetActive(true);
            activated = true;
            Time.timeScale = 0;
        }
    }

    public void StartGame()
    {
        mainMenuGUI.SetActive(false);
        activated = false;
        isFirstTime = false;
        EventSystem.current.SetSelectedGameObject(null);
        startButtonText.text = "Continue";
        Time.timeScale = 1;
    }

    public void RetartGame()
    {
        mainMenuGUI.SetActive(false);
        activated = false;
        isFirstTime = false;
        EventSystem.current.SetSelectedGameObject(null);
        startButtonText.text = "Start";
        Time.timeScale = 1;

        PlayerPrefs.SetInt("anger", 0);
        PlayerPrefs.SetInt("joy", 0);
        PlayerPrefs.SetInt("sadness", 0);
        PlayerPrefs.SetInt("fear", 0);
        PlayerPrefs.SetInt("videoPlayed", 0);

        SceneManager.LoadScene("HomeScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);
    }
}