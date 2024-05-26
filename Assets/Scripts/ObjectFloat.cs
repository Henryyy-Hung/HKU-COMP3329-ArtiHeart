using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public float amplitude = 1f; // 上下浮动的振幅
    public float frequency = 1f; // 浮动频率

    // 初始位置
    private Vector3 startPos;

    private void Start()
    {
        // 记录开始时的位置
        startPos = transform.position;
    }

    private void Update()
    {
        // 计算新的Y位置
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);
        // 更新物体的位置
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}