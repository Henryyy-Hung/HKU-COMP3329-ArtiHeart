using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIManagerJoy : MonoBehaviour
{
    public Image keyIcon1;
    public Image keyIcon2;

    GameObject[] imageObjects;

    private int keysCollected = 0;

    public GameObject passCanvas;
    public GameObject PassPanelPrefab;

    private GameObject PassPanelInstance;



    void Start()
    {
        //passCanvas.gameObject.SetActive(false);
        keyIcon1.gameObject.SetActive(false);
        keyIcon2.gameObject.SetActive(false);

        imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");

    }

    public void CollectKey()
    {
        AudioSource coinAudio = GetComponent<AudioSource>();
        coinAudio.Play();
        if (keysCollected == 0)
        {
            keyIcon1.gameObject.SetActive(true);
        }
        else
        {
            keyIcon2.gameObject.SetActive(true);
        }
        keysCollected++;
    }

    public int GetKeyNumber()
    {
        return keysCollected;
    }

    public void CheckPuzzleCompletion()
    {
        if (imageObjects == null || imageObjects.Length == 0)
        {
            imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
        }

        foreach (var piece in imageObjects)
        {
            if (!piece.GetComponent<RotatePuzzlePiece>().CheckIfCorrect())
            {
                return;
            }
        }
        Debug.Log("游戏结束");
        
        //QuitGame();
        GameObject chestInstance = GameObject.FindWithTag("Chest");
        chestInstance.GetComponent<Chest>().inactivePuzzlePanel();
        PassPanelInstance = Instantiate(PassPanelPrefab, passCanvas.transform);


    }
    public void QuitGame()
    {
        Application.Quit();
        // 如果在Unity编辑器中，可以添加以下行来模拟退出行为
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
