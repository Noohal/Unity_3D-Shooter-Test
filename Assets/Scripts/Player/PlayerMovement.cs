using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    [SerializeField] Animator playerAnimator;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float groundRange;
    [SerializeField] Transform hands;

    public float xRotation { get; set; } = 0f;
    public float yRotation { get; set; } = 0f;
    public float smoothXRot { get; private set; } = 0f;
    public float mouseSensitivity { get; private set; } = 150f;

    public bool isGrounded { get; private set; } = false;
    public bool isWalking { get; private set; } = false;
    public bool isJumping { get; private set; } = false;
    public bool isCrouching { get; private set; } = false;
    public bool normalFalling = true;

    public Camera cam;
    public Vector3 movementVector = Vector3.zero;
    [System.NonSerialized] public Vector3 jumpVelocity = Vector3.zero;

    private Vector3 originalCameraPos;
    private CharacterController characterController;
    private n_PlayerStats stats;
    private PlayerWeapon weapon;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        stats = GetComponent<n_PlayerStats>();
        weapon = GetComponent<PlayerWeapon>();
        //playerAnimator = GetComponentInChildren<Animator>();

        originalCameraPos = cam.transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, ground);

        HandleJump();
        HandleMovement();
        HandleCamera();
    }
    
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        isWalking = Input.GetKey(KeyCode.LeftShift);
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        if (isCrouching)
            HandleCrouch();
        else
            HandleStand();

        movementVector = Vector3.ClampMagnitude(transform.right * horizontal + transform.forward * vertical, 1.0f);
        
        if (isCrouching || characterController.height < stats.standingHeight)
            characterController.Move(movementVector * stats.crouchingMoveSpeed * Time.deltaTime);
        else if (isWalking)
            characterController.Move(movementVector * stats.walkingMoveSpeed * Time.deltaTime);
        else
            characterController.Move(movementVector * stats.runningMoveSpeed * Time.deltaTime);

        HandleAnimation(horizontal, vertical);
    }

    void HandleJump()
    {
        bool attemptingToJump = Input.GetKeyDown(KeyCode.Space);
        
        if(attemptingToJump && isGrounded)
            isJumping = true;
        else
            isJumping = false;

        if(isGrounded && jumpVelocity.y < 0f)
            jumpVelocity.y = -2f;

        if(isJumping)
            jumpVelocity.y = Mathf.Sqrt(stats.jumpHeight * -2f * stats.gravity);

        //if(normalFalling)
            //jumpVelocity.y += stats.gravity * Time.deltaTime;
        characterController.Move(jumpVelocity * Time.deltaTime);

    }

    void HandleCrouch() {
        if(characterController.height > stats.crouchHeight) {
            UpdateCharacterHeight(stats.crouchHeight, stats.cameraCrouchHeight);
            if(characterController.height - 0.05f <= stats.crouchHeight)
                characterController.height = stats.crouchHeight;
        }
    }

    void HandleStand() {
        if(characterController.height < stats.standingHeight) {
            float lastHeight = characterController.height;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.up, out hit, stats.standingHeight)) {
                if (hit.distance > stats.standingHeight - lastHeight) {
                    isCrouching = true;
                    return;
                } else {
                    UpdateCharacterHeight(stats.standingHeight, stats.cameraStandingHeight);
                }
            } else {
                UpdateCharacterHeight(stats.standingHeight, stats.cameraStandingHeight);
            }

            if (characterController.height + 0.001f >= stats.standingHeight) {
                characterController.height = stats.standingHeight;
            }
            transform.position += new Vector3(0, (characterController.height - lastHeight) / 2, 0);
        }
    }

    void UpdateCharacterHeight(float height, float camHeight) {
        characterController.height = Mathf.Lerp(characterController.height, height, stats.crouchSpeed * Time.deltaTime);
        float tempY = Mathf.Lerp(cam.transform.localPosition.y, camHeight, stats.crouchSpeed * Time.deltaTime);
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, tempY, cam.transform.localPosition.z);
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleAnimation(float hSpeed, float vSpeed)
    {
        playerAnimator.SetFloat("verticalSpeed", vSpeed);
        playerAnimator.SetFloat("horizontalSpeed", hSpeed);
        playerAnimator.SetBool("isJumping", isJumping);
        playerAnimator.SetBool("isCrouching", isCrouching);
    }

}
