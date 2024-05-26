using UnityEngine;

public class L4EnemyScript : MonoBehaviour
{
    // 公共变量
    public bool isDead = false; // 敌人是否死亡

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider obj)
    {
        // 子弹碰撞
        this.gameObject.GetComponent<Animator>().SetTrigger("isDead");
        isDead = true;
    }
}