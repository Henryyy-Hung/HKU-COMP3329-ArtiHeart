using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePuzzlePiece : MonoBehaviour
{

    private UIManagerJoy uIManager;

    // 响应点击事件
    void Start()
    {
        uIManager = Object.FindFirstObjectByType<UIManagerJoy>();
    }
    public void OnPieceClicked()
    {

            transform.Rotate(0f, 0f, 90f); // 旋转90度
            if (CheckIfCorrect()){
            InformUI();}
    }

    public bool CheckIfCorrect()
    {
        // 这里简单地检查Z轴旋转是否接近0度（或360的整数倍）
        Debug.Log("angle="+transform.eulerAngles.z+"  "+Mathf.Approximately((int) transform.eulerAngles.z % 360, 0));
        if (Mathf.Approximately((int) transform.eulerAngles.z % 360, 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        public void InformUI()
    {
        uIManager.CheckPuzzleCompletion();
    }

}
