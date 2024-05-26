using UnityEngine;

public class L4CubeMovement : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newX = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPosition + Vector3.forward * newX;
    }
}
