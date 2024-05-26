using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level3Manager : MonoBehaviour
{
    GameObject enemy;
    GameObject gm;
    GameObject[] imageObjects;

    public Button returnButton;
    public Text openChestText;
    public Text hintText;
    public Text StatusText;
    public GameObject blurMask;
    public GameObject puzzlePanel;
    public GameObject treasureChest;
    private bool instrcutionShow = false;
    private bool puzzleComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
        gm = GameObject.Find("gameManager");
        openChestText.gameObject.SetActive(false);
        hintText.gameObject.SetActive(false);
        puzzlePanel.SetActive(false);
        returnButton.gameObject.SetActive(false);
        treasureChest.SetActive(false);
        imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GetComponent<L4GameManagerScript>().bulletNumber <= 7)
        {
            hintText.gameObject.SetActive(true);
        }

        if (enemy.GetComponent<L4EnemyScript>().isDead && !instrcutionShow)
        {
            instrcutionShow = true;
            treasureChest.SetActive(true);
            openChestText.gameObject.SetActive(true);
            hintText.gameObject.SetActive(false);
            // openChestText.text = "Game completed! Press Space to open the treasure chest!";
        }

        if (treasureChest.GetComponent<LootBox>().isOpen && !puzzleComplete)
        {
            ShowPuzzlePanel();
        }
    }

    public void ShowPuzzlePanel()
    {
        hintText.gameObject.SetActive(false);
        blurMask.SetActive(true);
        puzzlePanel.SetActive(true);
        openChestText.gameObject.SetActive(false);
    }

    public void CheckPuzzleCompletion()
    {
        if (imageObjects ==null || imageObjects.Length == 0){
            imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
        }
        foreach (var piece in imageObjects)
        {
            if (!piece.GetComponent<L4RotatePuzzlePiece>().CheckIfCorrect()){
                return;
            }
        }
        
        // 获得anger
        PlayerPrefs.SetInt("anger", 1);
        puzzleComplete = true;
        puzzlePanel.SetActive(false);
        StatusText.text = "Puzzle Completed!\nEmotion Anger Collected!";
        StatusText.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);
        returnButton.onClick.AddListener(ReturntoMainMenu);
    }

    void ReturntoMainMenu()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Bgm"));
        SceneManager.LoadScene("HomeScene");
    }
}
