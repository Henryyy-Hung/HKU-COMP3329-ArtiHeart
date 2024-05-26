using UnityEngine;

public class L4ArmorSupplyScript : MonoBehaviour
{
    public bool isHit = false;

    void Start()
    {
        Transform broken = transform.Find("Destroy_box");
        broken.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        // 子弹碰撞
        if (isHit) return;
        Transform broken = transform.Find("Destroy_box");
        Transform intact = transform.Find("Box001");
        intact.gameObject.SetActive(false);
        broken.gameObject.SetActive(true);
        this.gameObject.
        GetComponent<Animator>().SetTrigger("isHit");
        isHit = true;
    }
}
