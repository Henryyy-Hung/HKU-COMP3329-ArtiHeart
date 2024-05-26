using UnityEngine;
using UnityEngine.AI; // Include the AI namespace

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    public float moveSpeed = 5.0f; // You can adjust the move speed as needed
    public Camera cam; // Reference to the main camera
    public float tiltAngle = 20.0f; // Angle at which the player will tilt when above SkyPlane
    public float hoverHeight = 0.3f; // The height range within which the player will hover
    public GameObject wing;

    private float originalY; // The original y position of the player
    private Animator animator;
    private float flyStartTime;

    public bool moveDisabled;

    // Start is called before the first frame update
    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        playerAgent.updateRotation = false; // We'll handle rotation manually
        originalY = transform.position.y; // Save the original y position
        animator = GetComponent<Animator>();
        flyStartTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        DialogueSystem dialogueSystem = FindObjectOfType<DialogueSystem>();
        MainMenu mainMenu = FindObjectOfType<MainMenu>();

        if (dialogueSystem.isTyping || mainMenu.activated)
        {
            moveDisabled = true;
        }
        else
        {
            moveDisabled = false;
        }

        // Get input from the horizontal and vertical axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Check for ground and sky plane
        bool isAboveSkyPlane = CheckForObjectBelow("SkyPlane");
        bool isAboveGround = CheckForObjectBelow("Ground");

        // If there's any input
        if (inputDirection.magnitude >= 0.1f && !moveDisabled)
        {
            // Calculate the direction relative to the camera's rotation
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Set the velocity of the navmesh agent
            playerAgent.velocity = moveDirection.normalized * moveSpeed;

            // Set the NavMeshAgent destination
            playerAgent.SetDestination(transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime);

            // Determine the correct rotation based on the ground and sky plane
            Quaternion targetRotation;
            if (isAboveGround)
            {
                targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                animator.SetBool("IsWalking", true);
                moveSpeed = 8;
                wing.SetActive(false);

                flyStartTime = 0f;

            }
            else if (isAboveSkyPlane)
            {
                animator.SetBool("IsWalking", false);
                moveSpeed = 20;
                wing.SetActive(true);

                if (flyStartTime == 0f)
                {
                    flyStartTime = Time.time;
                }

                targetRotation = Quaternion.Euler(tiltAngle, targetAngle, 0f);
            }
            else
            {
                targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            }

            // Smoothly rotate the player to face the direction of movement
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerAgent.angularSpeed);
        }
        else
        {
            // Stop the movement when there is no input
            playerAgent.velocity = Vector3.zero;
            animator.SetBool("IsWalking", false);
        }

        // Hover effect when above SkyPlane and not above Ground
        if (isAboveSkyPlane && !isAboveGround)
        {
            // Use a sine wave to calculate the hover effect
            float hoverDelta = Mathf.Sin((Time.time - flyStartTime) * 2 * Mathf.PI * 0.5f + Mathf.PI / 2) * hoverHeight;

            //Debug.Log(hoverDelta);

            transform.position = new Vector3(transform.position.x, originalY + 0.5f + hoverDelta, transform.position.z);

            // If not moving, tilt the player if above SkyPlane
            // if (inputDirection.magnitude < 0.1f)
            // {
            transform.rotation = Quaternion.Euler(tiltAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            // }
        }
        else
        {
            // Reset y position when not above SkyPlane
            transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
        }
    }

    private bool CheckForObjectBelow(string objectTag)
    {
        RaycastHit hit;

        // Cast a ray downwards to detect if we are above a specific tagged object
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            // Check if the object hit has the specific tag
            return hit.collider.CompareTag(objectTag);
        }

        return false;
    }
}