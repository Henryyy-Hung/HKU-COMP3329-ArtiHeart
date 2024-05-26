using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2shoot : MonoBehaviour
{
    public GameObject Bullet;
    public Transform gunbullet;
    public Transform parentt;
    public int bulletnum = 10;
    public double shs = 0.2f;
    private float timee = 0;
    public float bulletsd = 60f;
    private GameObject currentbullet;
    AudioSource Aud;
    public AudioClip shoott;

    // Start is called before the first frame update
    void Start()
    {
        parentt = transform.parent;
        Aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
 //     Vector3 local = transform.forward;
 //     Vector3 world = transform.TransformDirection(local);
        timee = timee + Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow) && timee > shs)
        {
            timee = 0;
            currentbullet=Instantiate(Bullet, gunbullet.position, gunbullet.rotation) as GameObject;
            currentbullet.GetComponent<Rigidbody>().velocity = parentt.forward * bulletsd;
            Aud.clip = shoott;
            Aud.Play();
            if (Aud.isPlaying)
            {
                Debug.Log("shot");
            }
            Destroy(currentbullet, 2);
        }
    }
    
}
    