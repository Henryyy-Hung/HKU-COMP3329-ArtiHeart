using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    //public GameObject keyIcon; // 在编辑器中设置为钥匙图标的GameObject

    // 开始时不显示钥匙图标
    private GameObject player;
    private UIManagerJoy UImanager;

    

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        UImanager = FindFirstObjectByType<UIManagerJoy>();

    }
    void Update()
    {
        if ((int)(player.transform.position.x + 0.5) == (int)(transform.position.x + 0.5) & (int)(player.transform.position.z + 0.5) == (int)(transform.position.z + 0.5))
        {

            UImanager.CollectKey();
            Destroy(gameObject);

        }


    }
    // 调用此方法来显示钥匙图标

}
