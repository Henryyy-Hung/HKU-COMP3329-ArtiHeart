using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2RotatePuzzlePiece : MonoBehaviour
{

    private L2UIManager uIManager;

    // 响应点击事件
    void Start()
    {
        uIManager = Object.FindFirstObjectByType<L2UIManager>();
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
            //Debug.Log("Mathf.Approximately="+Mathf.Approximately(transform.eulerAngles.z % 360, 0)+"  Angle="+transform.eulerAngles.z);
            return true;
            // 检查所有拼图块是否正确
            
        }
        else
        {
            //Debug.Log("false");
            return false;
        }
    }
        public void InformUI()
    {
        uIManager.CheckPuzzleCompletion();
    }

}
