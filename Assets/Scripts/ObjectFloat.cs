using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public float amplitude = 1f; // ���¸��������
    public float frequency = 1f; // ����Ƶ��

    // ��ʼλ��
    private Vector3 startPos;

    private void Start()
    {
        // ��¼��ʼʱ��λ��
        startPos = transform.position;
    }

    private void Update()
    {
        // �����µ�Yλ��
        float newY = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);
        // ���������λ��
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}