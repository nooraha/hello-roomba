using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent<float> playerJumped = new UnityEvent<float>();
    Rigidbody rb;
    [SerializeField] GameObject cameraObj;

    float mouseXInput, mouseYInput;
    Vector2 movementInputs;
    Vector3 cameraRotation, playerRotation, playerMovement;

    float walkSpeed = 5f;
    float rotationSensitivity = 5f;
    float cameraXAxisClamp;

    float maxJumpHeight = 7f;
    float maxJumpChargingTime = 0.5f;
    float timeSpaceBarDown = 0f;
    bool jumpCharging = false;

    public bool allowedToMove = true;

    PlayerHunger playerHunger;

    void Awake()
    {
        playerHunger = FindFirstObjectByType<PlayerHunger>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movementInputs.x = Input.GetAxis("Horizontal");
        movementInputs.y = Input.GetAxis("Vertical");
        mouseXInput = Input.GetAxis("Mouse X");
        mouseYInput = Input.GetAxis("Mouse Y");

        if (allowedToMove)
        {
            DoRotation();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            timeSpaceBarDown += Time.deltaTime;
            jumpCharging = true;
        }
        else
        {
            if (jumpCharging)
            {
                jumpCharging = false;

                DoJump();
            }
        }
    }

    void FixedUpdate()
    {
        if (allowedToMove)
        {
            DoMovement();
        }

        
    }

    void DoJump()
    {
        // compute height of jump
        // jumpStrength is decimal 0.0-1.0 describing strength of jump in comp. to max allowed
        float jumpStrength = Mathf.Min(maxJumpChargingTime, timeSpaceBarDown + 0.1f) / maxJumpChargingTime;

        if (playerHunger.currentHunger < jumpStrength * playerHunger.maxHungerDecayFromJumping)
        {
            jumpStrength = playerHunger.currentHunger / playerHunger.maxHungerDecayFromJumping;
        }
        
        Vector3 jumpForce = Vector3.up * maxJumpHeight * jumpStrength;
        rb.AddForce(jumpForce, ForceMode.Impulse);

            timeSpaceBarDown = 0f;
            playerJumped.Invoke(jumpStrength);
        
    }

    void DoMovement()
    {
        // clamp inputs
        if (movementInputs.x + movementInputs.y > 0.7f)
        {
            movementInputs = Vector2.ClampMagnitude(movementInputs, 1);
        }

        playerMovement = (transform.forward * movementInputs.y + transform.right * movementInputs.x) * walkSpeed;
        playerMovement.y = rb.linearVelocity.y;

        rb.MovePosition(transform.position + playerMovement * Time.deltaTime);
    }

    void DoRotation()
    {
        // get current rotation
        cameraRotation = cameraObj.transform.rotation.eulerAngles;
        playerRotation = transform.rotation.eulerAngles;

        // change rotation from input
        cameraRotation.x -= mouseYInput * rotationSensitivity;
        cameraRotation.z = 0;
        playerRotation.y += mouseXInput * rotationSensitivity;

        // stop camera from flipping ?
        cameraXAxisClamp -= mouseYInput * rotationSensitivity;
        if (cameraXAxisClamp > 90)
        {
            cameraXAxisClamp = 90;
            cameraRotation.x = 90;
        }
        else if (cameraXAxisClamp < -90)
        {
            cameraXAxisClamp = -90;
            cameraRotation.x = 270;
        }

        // apply rotation
        cameraObj.transform.rotation = Quaternion.Euler(cameraRotation);
        transform.rotation = Quaternion.Euler(playerRotation);
    }

    public void StopMovement(float seconds)
    {
        allowedToMove = false;
        rb.linearVelocity = Vector3.zero;
        Invoke("ReallowMovement", seconds);
    }
    
    public void ReallowMovement()
    {
        allowedToMove = true;
    }
}
