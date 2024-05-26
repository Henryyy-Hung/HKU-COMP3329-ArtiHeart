using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class L2gamemenu : MonoBehaviour
{
    public GameObject menupanel;
    public GameObject pasbt;
    public TextMeshProUGUI keynum;
    // Start is called before the first frame update
    void Start()
    {
        menupanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        keyui();
    }
    public void paus()
    {
        Time.timeScale = 0;
        pasbt.SetActive(false);
        menupanel.SetActive(true);
    }
    public void contin()
    {
        Time.timeScale = 1f;
        pasbt.SetActive(true);
        menupanel.SetActive(false);
    }
    public void qut()
    {
        keytext.key = 0;
        SceneManager.LoadScene("HomeScene");
        Time.timeScale = 1;

    }
    private void keyui()
    {
        keynum.text = "KEYs collected: " + keytext.key.ToString();
    }

}
