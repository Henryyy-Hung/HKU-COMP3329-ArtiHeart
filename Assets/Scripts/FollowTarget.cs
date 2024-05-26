using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float pitch = 2f;
    public float minPitch = -10f;
    public float maxPitch = 10f;
    public float pitchSpeed = 100f;
    public float yawSpeed = 100f;

    private float currentZoom = 10f;
    private float currentYaw = 0f;
    private float currentPitch = 0f;

    // Add new properties for handling obstacles
    public LayerMask obstacleLayers; // The layers that are considered as obstacles
    public float cameraCollisionOffset = 0.3f; // Offset to prevent camera clipping into obstacles


    public float angle = 2.0f; // Maximum angle the camera will rotate from its original orientation.
    public float speed = 0.3f; // Speed of rotation.

    private Quaternion startRotation; // The original rotation of the camera.

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        MainMenu mainMenu = FindObjectOfType<MainMenu>();

        if (mainMenu.activated)
        {
            if (mainMenu.isFirstTime)
            {
                // Calculate the new rotation angle.
                float rotationAngle = Mathf.Sin(Time.time * speed) * angle;
                // Create a quaternion representing the rotation around the Y axis (assuming Y is your up axis).
                Quaternion newRotation = Quaternion.Euler(rotationAngle, 0f, 0f);
                // Combine the original rotation with the new rotation.
                transform.rotation = startRotation * newRotation;
            }
            return;
        }

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;

        currentPitch -= Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
    }

    void LateUpdate()
    {
        MainMenu mainMenu = FindObjectOfType<MainMenu>();

        if (mainMenu.activated)
        {
            return;
        }

        // Calculate the new position of the camera
        Vector3 dir = new Vector3(0, 0, -currentZoom);
        Quaternion rotation = Quaternion.Euler(currentPitch + pitch, currentYaw, 0);
        Vector3 targetPosition = target.position - rotation * dir + offset;

        // Check for obstacles
        RaycastHit hit;
        if (Physics.Raycast(target.position, targetPosition - target.position, out hit, currentZoom, obstacleLayers))
        {
            // If there is an obstacle, calculate a closer position for the camera
            targetPosition = target.position + (targetPosition - target.position).normalized * (hit.distance - cameraCollisionOffset);
        }

        // Update camera position
        transform.position = targetPosition;

        // Ensure the camera is always looking at the target
        transform.LookAt(target.position + Vector3.up * pitch);
    }
}