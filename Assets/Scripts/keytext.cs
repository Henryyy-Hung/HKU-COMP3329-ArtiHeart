using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keytext: MonoBehaviour
{
    
    public static int key = 0;
    public float rotationsd = 150.0f;
 //   public Text KeyText;
    // Start is called before the first frame update
    void Start()
    {
        //       KeyText.text = "Key Obtained: " + 0;


    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xAngle: 0, yAngle: rotationsd * Time.deltaTime, zAngle: 0);
    }


    void OnTriggerEnter(Collider xx)
    {
        if (xx.transform.tag == "playyer")
        {
            key = key + 1;

            //         KeyText.text = "Key obtained: " + key;
            Destroy(this.gameObject);
        }
    }
    
}
