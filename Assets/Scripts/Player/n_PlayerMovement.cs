using UnityEngine;

public class n_PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask groundLayer;
    public Transform groundCheck;
    
    public float xRotation;
    public float yRotation;

    private bool isCrouching = false;
    public bool isGrounded = false;
    public bool normalFalling = true;
    private bool isJumping = false;

    private float groundRange = 0.4f;

    public Vector3 jumpVelocity = Vector3.zero;
    public Vector3 movementVector = Vector3.zero;

    private KeyCode walkButton = KeyCode.LeftShift;
    private KeyCode jumpButton = KeyCode.Space;
    private KeyCode crouchButton = KeyCode.LeftControl;

    private n_PlayerStats playerStats;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<n_PlayerStats>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRange, groundLayer);

        HandleMovement();
        HandleJump();
        HandleCamera();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isWalking = Input.GetKey(walkButton);
        isCrouching = Input.GetKey(crouchButton);

        if(isCrouching)
        {
            Crouch();
        } else
        {
            Stand();
        }

        movementVector = Vector3.ClampMagnitude(transform.right * horizontal + transform.forward * vertical, 1f);
        if(isCrouching)
        {
            characterController.Move(movementVector * playerStats.crouchingMoveSpeed * Time.deltaTime);
        }
        else if (isWalking)
        {
            characterController.Move(movementVector * playerStats.walkingMoveSpeed * Time.deltaTime);
        } 
        else
        {
            characterController.Move(movementVector * playerStats.runningMoveSpeed * Time.deltaTime);
        }
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * playerStats.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * playerStats.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        bool attemptingJump = Input.GetKeyDown(jumpButton);

        if(attemptingJump && isGrounded)
        {
            isJumping = true;
        } else
        {
            isJumping = false;
        }

        if (isGrounded && jumpVelocity.y < 0f)
        {
            jumpVelocity.y = -2f;
        }

        if(isJumping)
        {
            jumpVelocity.y = Mathf.Sqrt(playerStats.jumpHeight * playerStats.gravity * -2f);
        }

        if(normalFalling)
        {
            jumpVelocity.y += playerStats.gravity * Time.deltaTime;
        }
        characterController.Move(jumpVelocity * Time.deltaTime);
    }

    void Crouch()
    {
        if(characterController.height <= playerStats.crouchHeight)
        {
            return;
        }

        UpdateCharacterHeight(playerStats.crouchHeight);
        UpdateCameraHeight(playerStats.cameraCrouchHeight);

        if (characterController.height + 0.05f <= playerStats.crouchHeight)
        {
            characterController.height = playerStats.crouchHeight;
        }
    }

    void Stand()
    {
        if(characterController.height >= playerStats.standingHeight)
        {
            return;
        }

        float lastHeight = characterController.height;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.up, out hit, playerStats.standingHeight))
        {
            if (playerStats.crouchHeight + hit.distance > playerStats.standingHeight)
            {
                UpdateCharacterHeight(playerStats.crouchHeight + hit.distance);
                UpdateCameraHeight(playerStats.cameraCrouchHeight);
                return;
            }
        } else
        {
            UpdateCharacterHeight(playerStats.standingHeight);
            UpdateCameraHeight(playerStats.cameraStandingHeight);
        }

        if (characterController.height + 0.05f >= playerStats.standingHeight)
        {
            characterController.height = playerStats.standingHeight;
        }

        transform.position += new Vector3(0, (characterController.height - lastHeight) / 2, 0);
    }

    private void UpdateCharacterHeight(float newHeight)
    {
        characterController.height = Mathf.Lerp(characterController.height, newHeight, playerStats.crouchSpeed * Time.deltaTime);
    }

    private void UpdateCameraHeight(float newHeight)
    {
        float newCameraHeight = Mathf.Lerp(playerCamera.transform.localPosition.y, newHeight, playerStats.crouchSpeed * Time.deltaTime);
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, newCameraHeight, playerCamera.transform.localPosition.z);
    }
}
