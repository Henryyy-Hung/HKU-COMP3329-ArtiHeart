using UnityEngine;

public class L4BulletScript: MonoBehaviour
{
    GameObject gm;

    public float bulletSpeed = 10.0f; // 子弹速度
    public float bulletLifetime = 5.0f; // 子弹生命周期
    private Rigidbody rb;
    private Vector3 facing_pos; // 子弹射向的方向

    void Start()
    {
        gm = GameObject.Find("gameManager");
        gm.GetComponent<L4GameManagerScript>().bulletNumber -= 1;

        rb = GetComponent<Rigidbody>();
        // 子弹以初速度射向鼠标
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 player_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.z = player_pos.z;
        facing_pos = Camera.main.ScreenToWorldPoint(mouse_pos);
        Vector3 initialVelocity = (facing_pos - transform.position).normalized * bulletSpeed;
        rb.velocity = initialVelocity;

        // 设置子弹的生命周期
        Destroy(gameObject, bulletLifetime);
    }

    void Update()
    {
        if (!gm.GetComponent<L4GameManagerScript>().isStart)
        {
            Destroy(gameObject);
        }
    }
}