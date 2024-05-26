using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2monster : MonoBehaviour
{
    public float ensd;
    public Transform thepos;
    private float xmin;
    private float zmin;
    private float xmax;
    private float zmax;
    private float wait;
    private float start;
    // Start is called before the first frame update
    void Start()
    {
        wait = start;
        xmin = transform.position.x - 15;
        xmax = transform.position.x + 15;
        zmin = transform.position.z - 15;
        zmax = transform.position.z + 15;
        thepos.position = new Vector3(Random.Range(xmin, xmax),0, Random.Range(zmin, zmax));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, thepos.position, ensd * Time.deltaTime);
        if (Vector3.Distance(transform.position, thepos.position) < 0.01 && wait <= 0)
        {
            thepos.position= new Vector3(Random.Range(xmin, xmax), 0, Random.Range(zmin, zmax));
            wait = start;
        }
        else if (Vector3.Distance(transform.position, thepos.position) < 0.01 && wait > 0)
        {
            wait = wait - Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider aaa)
    {
        if (aaa.gameObject.tag != "playyer" && aaa.gameObject.tag != "bullet")
        {
            if(wait <= 0)
            {
                thepos.position = new Vector3(Random.Range(xmin, xmax), 0, Random.Range(zmin, zmax));
                Debug.Log("change");
                wait = start;
            }
            else if (wait > 0)
            {
                wait = wait - Time.deltaTime;
            }
        }
        else if (aaa.gameObject.tag == "playyer")
        {
            //       Debug.Log("Hit");
            Time.timeScale = 0;
        }
        else if (aaa.gameObject.tag == "bullet")
        {
            Destroy(gameObject);
        }
    }
}
