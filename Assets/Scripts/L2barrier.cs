using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2barrier : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider lll)
    {
        if(lll.gameObject.tag!="bullet"&&lll.gameObject.tag!="monster")
        {
            
            //       Debug.Log("Hit");
            Time.timeScale = 0;
        }
    }
}
