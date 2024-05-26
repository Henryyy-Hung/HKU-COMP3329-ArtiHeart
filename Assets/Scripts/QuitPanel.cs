using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitPanel : MonoBehaviour
{
    // Start is called before the first frame update

    public Button returnButton;
    public Button returnCancelButton;

    

    public GameObject quitPanel;
    void Start()
    {
        returnButton.onClick.AddListener(ClickReturn);
        returnCancelButton.onClick.AddListener(clickReturnCancel);
        //quitPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickReturn(){
        SceneManager.LoadScene("HomeScene");
    }
    public void clickReturnCancel(){
        quitPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
