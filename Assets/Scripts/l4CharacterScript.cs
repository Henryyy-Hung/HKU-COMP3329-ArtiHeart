using UnityEngine;

public class L4CharacterScript : MonoBehaviour
{
    // GameObject enemy;
    GameObject gm;

    // 公共变量
    public GameObject bulletPrefab; // 子弹预制体
    public AudioSource shootingSound; // 射击音效
    public bool isDead = false; // 玩家是否死亡
    // 私有变量
    private Vector3 facing_pos; // 角色面向的位置
    private int hitPlayerCount = 0; // 子弹击中玩家次数
    private int mouseClickCount = 0; // 鼠标点击次数
    private bool isShooting = false; // 是否正在射击

    void Start()
    {
        gm = GameObject.Find("gameManager");
    }

    void Update()
    {
        if (!isDead) {
            // 检测鼠标点击（左键）
            if (Input.GetMouseButtonDown(0) && gm.GetComponent<L4GameManagerScript>().isStart && gm.GetComponent<L4GameManagerScript>().bulletNumber > 0 && Time.timeScale != 0) {
                isShooting = true;
            }
        }
    }

    void FixedUpdate()
    {
        // 角色面向鼠标位置
        Vector3 mouse_pos = Input.mousePosition;
        Vector3 player_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.z = player_pos.z;
        facing_pos = Camera.main.ScreenToWorldPoint(mouse_pos);
        transform.LookAt(facing_pos);

        // 发射子弹
        if (isShooting)
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("mouseClick");
            mouseClickCount++;
            ShootBullet();
            isShooting = false;
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        // 射出时触发
        if (hitPlayerCount <= mouseClickCount) {
            hitPlayerCount++;
        }
        // 再次碰撞
        else {
            this.gameObject.GetComponent<Animator>().SetTrigger("isDead");
            isDead = true;
        }
    }

    void ShootBullet()
    {
        // 在角色位置实例化一个子弹
        shootingSound.Play();
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}