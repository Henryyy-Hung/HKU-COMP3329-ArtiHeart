using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextBotton : MonoBehaviour
{
    // Start is called before the first frame update
    public Button NextButton;
    void Start()
    {
        NextButton.onClick.AddListener(ClickNext);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickNext(){
        PlayerPrefs.SetInt("joy", 1);
        SceneManager.LoadScene("HomeScene");
    }
}
