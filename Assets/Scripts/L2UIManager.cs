using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2UIManager : MonoBehaviour
{


    GameObject[] imageObjects;
    public GameObject cv3;
    public GameObject cv4;
    private int quitGame;
    // Start is called before the first frame update
    void Start()
    {
        cv3.SetActive(false);
        cv4.SetActive(false);
        //imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
        if (imageObjects == null || imageObjects.Length == 0)
        {
            imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
            Debug.Log("seccess");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CheckPuzzleCompletion()
    {
        if (imageObjects == null || imageObjects.Length == 0)
        {
            imageObjects = GameObject.FindGameObjectsWithTag("PuzzleImage");
        }
        //Debug.Log("number="+imageObjects.Length);
        foreach (var piece in imageObjects)
        {
            Debug.Log("for");
            //piece.GetComponent<RotatePuzzlePiece>().CheckIfCorrect();
            if (!piece.GetComponent<L2RotatePuzzlePiece>().CheckIfCorrect())
            {
                return;
            }
        }
        cv4.SetActive(false);
        cv3.SetActive(true);
        PlayerPrefs.SetInt("fear", 1);
        Debug.Log("”Œœ∑Ω· ¯");
        Time.timeScale = 0;
    }
}
