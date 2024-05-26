using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class L2player : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pausebuttonn;
    public GameObject cv2;
    public GameObject cv4;
    public static int key = 0;
    public float hxsd = 5;
    public float zxsd = 30;
    public float rotasd = 20;
    public float jpgravity = 300;
    public Rigidbody rb;
    public int gameend = 0;
    private bool ongrd = true;
    AudioSource Aud;
    public AudioClip coll;
    void Start()
    {
        pausebuttonn.SetActive(true);
        rb = this.GetComponent<Rigidbody>();
        cv2.SetActive(false);
        cv4.SetActive(false);
        Aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            remake();
        float hori = Input.GetAxis("Horizontal");
        if (ongrd == true)
        {
            
            transform.Translate(hori * hxsd * Time.deltaTime, 0, zxsd * Time.deltaTime);
            transform.Rotate(0, hori * rotasd * Time.deltaTime, 0);
        }
        else
        {
            transform.Translate(hori * hxsd * Time.deltaTime, 0, (zxsd - 7) * Time.deltaTime);
            transform.Rotate(0, hori * rotasd * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Space) && ongrd == true)
        {
            rb.AddForce(Vector3.up * jpgravity);
            ongrd = false;
        }
        //       downn();

    }

    private void OnCollisionEnter(Collision gdt)
    {
        if (gdt.gameObject.tag == "Groundd")
        {
            ongrd = true;
        }
        if (gdt.gameObject.tag == "gamed2")
        {
            Invoke("gmed", 1);
        }
    }
    private void OnTriggerEnter(Collider kk)
    {
        if (kk.gameObject.tag != "gamed" && kk.gameObject.tag != "gamed2" && kk.gameObject.tag != "keyy" && kk.gameObject.tag != "bullet")
        {
            pausebuttonn.SetActive(false);
            cv2.SetActive(true);
            Time.timeScale = 0;
        }
        if (kk.gameObject.tag == "keyy")
        {
            Aud.clip = coll;
            Aud.Play();
        }
    }



    /*    void downn()
        {
            if (transform.position.y <= -20)
            {
                end();
                gameend = 1;
                return;
            }
        }*/
    void remake()
    {
        keytext.key = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1;
    }
    void end()
    {
        Time.timeScale = 0;
    }
    void contin()
    {
        Time.timeScale = 1;
    }
    void gmed()
    {
        zxsd = 0;
        cv4.SetActive(true);
 //       Time.timeScale = 0;
    }


}