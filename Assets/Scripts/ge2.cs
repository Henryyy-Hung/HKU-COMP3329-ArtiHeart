using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ge2 : MonoBehaviour
{
    public GameObject cv;
    public int passkeynum;
    // Start is called before the first frame update
    void Start()
    {
        cv.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider yy)
    {
        if(yy.tag=="playyer" && keytext.key<passkeynum)
        {
            cv.SetActive(true);
            Time.timeScale = 0;

 //           endlevel1();
        }
    }
 /*   private void UII(string ms)
    {
        getext.text = ms;
        gePanel.SetActive(true);
    }*/
  /*  void OnGUI()
    {
        GUI.skin = theSkin;
        if (tet == 1)
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(Screen.width / 2-100, Screen.height / 2-50, 200, 100), "No key! Press Q to restart");
        }
 
    }*/

 /*  private void endlevel1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }*/

}
