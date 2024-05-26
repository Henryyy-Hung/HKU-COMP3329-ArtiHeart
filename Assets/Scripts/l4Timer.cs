using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
    GameObject gm;

    public Text StatusText;
    public Text timerText;
    public Button restartButton;
    public GameObject blurMask;
    public float timeRemaining = 60;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindWithTag("Enemy");
        gm = GameObject.Find("gameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<L4CharacterScript>().isDead && !enemy.GetComponent<L4EnemyScript>().isDead && gm.GetComponent<L4GameManagerScript>().isStart)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = "Time: " + timeRemaining.ToString("F0");
            }
            else
            {
                TimeUp();
            }
        }
    }

    void TimeUp()
    {
        gm.GetComponent<L4GameManagerScript>().isStart = false;
        StatusText.text = "Time's Up.\nLevel failed!";
        restartButton.gameObject.SetActive(true);
        restartButton.onClick.AddListener(RestartLevel);
        StatusText.gameObject.SetActive(true);
        blurMask.SetActive(true);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
