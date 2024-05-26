using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    public GameObject quitPanel;
        private void Start()
    {
        GetComponent<Button>().onClick.AddListener(openQuitPanel);
         quitPanel.SetActive(false);
    }
    public void openQuitPanel(){
        quitPanel.SetActive(true);
        Time.timeScale = 0f;
        
    }
}
