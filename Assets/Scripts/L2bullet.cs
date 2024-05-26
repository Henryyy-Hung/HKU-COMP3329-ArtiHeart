using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2bullet : MonoBehaviour
{
    AudioSource Aud;
    public AudioClip dt;
    // Start is called before the first frame update
    void Start()
    {
        Aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider ee)
    {
        if (ee.gameObject.tag != "playyer" && ee.gameObject.tag !="monster")
        {
            Destroy(gameObject);
            Debug.Log("hit bullet");
        }
        else if (ee.gameObject.tag == "monster")
        {
            Aud.clip = dt;
            Aud.Play();
        }
        
    }

}
