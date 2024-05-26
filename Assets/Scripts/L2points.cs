using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2points : MonoBehaviour
{
    public Transform[] generatepoints;
    public float generatetime = 1f;
    public GameObject thekeys;
    public int tarkeynum=4;
    private int[] checkk = new int[200];
    // Start is called before the first frame update
    void Start()
    {
 /*       for(int i = 0; i <= 3; i++)
        {
            kkeys();
        }*/
        for(int ini = 0; ini < checkk.Length; ini++)
        {
            checkk[ini] = 0;
        }
 
        while (tarkeynum != 0)
        {
            int currentpos = rando();
            if (checkk[currentpos] == 0)
            {
                checkk[currentpos] = 1;
                kkeys(currentpos);
            }
            else if (checkk[currentpos] == 1)
            {
                tarkeynum += 1;
            }
            tarkeynum--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int rando()
    {
        int num = Random.Range(0, generatepoints.Length);//Ëæ»ú×ø±ê
        return num;
    }
    void kkeys(int posr)
    {
        Instantiate(thekeys, generatepoints[posr].position, generatepoints[posr].rotation);
    }
}
