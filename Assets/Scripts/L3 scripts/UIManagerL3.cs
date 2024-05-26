using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIManagerL3 : MonoBehaviour
{
    //public Image keyIcon1; 
    //public Image keyIcon2;
    public GameObject Chest;
    private ChestL3 chest_script;
    GameObject[] imageObjects;

    //private int keysCollected = 0;
    private int quitGame;


    void Start()
    {

        //keyIcon1.gameObject.SetActive(false);
        //keyIcon2.gameObject.SetActive(false);

        imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
        chest_script = Chest.GetComponent<ChestL3>();
    }

    //public void CollectKey()
    //{
    //    if (keysCollected == 0)
    //    {
    //        keyIcon1.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        keyIcon2.gameObject.SetActive(true);
    //    }
    //    keysCollected++;
    //}

    //public int GetKeyNumber()
    //{
    //    return keysCollected;
    //}

    public void CheckPuzzleCompletion()
    {
        if (imageObjects ==null || imageObjects.Length == 0){
            imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
        }
        //Debug.Log("number="+imageObjects.Length);
        foreach (var piece in imageObjects)
        {
            //piece.GetComponent<RotatePuzzlePiece>().CheckIfCorrect();
            if (!piece.GetComponent<RotatePuzzlePieceL3>().CheckIfCorrect()){
                return;
            }
        }
        chest_script.puzzling = false;
        Debug.Log("游戏结束");
        PlayerPrefs.SetInt("sadness", 1);
        //QuitGame();
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
