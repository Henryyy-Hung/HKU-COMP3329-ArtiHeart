using UnityEngine;

public class L4RotatePuzzlePiece : MonoBehaviour
{
    GameObject level3Manager;

    // 响应点击事件
    void Start()
    {
        level3Manager = GameObject.Find("level3Manager");
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
        if (Mathf.Approximately((int) transform.eulerAngles.z % 360, 0))
        {
            return true;
            // 检查所有拼图块是否正确
        }
        else
        {
            return false;
        }
    }
    public void InformUI()
    {
        level3Manager.GetComponent<Level3Manager>().CheckPuzzleCompletion();
    }
}
