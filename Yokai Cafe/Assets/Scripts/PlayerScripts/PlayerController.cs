using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

interface IInteractable
{
    string InteractName { get; }
    string InteractPrompt { get; }

    void Interact();
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    [Header("Fear")]
    [SerializeField] private float maxFear = 5f;
    [SerializeField] private float currentFear;

    public float MaxFear => maxFear;
    public float CurrentFear => currentFear;

    [Header("Health")]
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private float currentHealth;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 5f;
    [SerializeField] private float currentStamina;

    public float MaxStamina => maxStamina;
    public float CurrentStamina => currentStamina;

    public float staminaDrain = 1.2f;
    public float staminaRegen = 0.8f;
    public float regenDelay = 1.5f;
    private float regenTimer;
    private bool isSprinting;

    public float jumpHeight = 0f;
    public float gravity = -9.81f;

    [Header("Crouch")]
    private float originalHeight;
    private float originalCenterY;
    public float crouchHeight = 0.5f;
    public float crouchSpeed = 2f;

    [Header("Camera Settings")]
    public Camera playerCamera;
    public float lookSensitivity = 0.05f;
    public float lookXLimit = 75f;

    private CharacterController controller;
    private Vector3 velocity;
    private float rotationX = 0f;

    // New: Input System
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private bool runInput;
    private bool crouchInput;

    // For Interactables

    [Header("Interaction Settings")]
    public float interactCooldown = 0.25f; // time in seconds between interactions
    private float lastInteractTime = -Mathf.Infinity; // tracks last interaction
    public Transform InteractorSource;
    public float InteractRange = 5f;
    [SerializeField] private PlayerHoldController holdController;

    [Header("UI")]
    public GameObject interactPrompt;
    public TMP_Text interactText;

    private IInteractable currentInteractable;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        //Sprinting
        currentStamina = maxStamina;

        //Health
        currentHealth = maxHealth;

        //Fear
        currentFear = 0;

        // Crouching
        originalHeight = controller.height;
        originalCenterY = controller.center.y;

        // Initialize Input System
        inputActions = new InputSystem_Actions();

        // Bind input callbacks
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpInput = true;
        inputActions.Player.Sprint.performed += ctx => runInput = true;
        inputActions.Player.Sprint.canceled += ctx => runInput = false;

        inputActions.Player.Crouch.performed += ctx => crouchInput = true;
        inputActions.Player.Crouch.canceled += ctx => crouchInput = false;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleCrouch();
        HandleInteract();
        HandleDrop();
    }
    void HandleCrouch()
    {
        if (crouchInput)
        {
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * crouchSpeed);

            controller.center = new Vector3(controller.center.x, Mathf.MoveTowards(controller.center.y, originalCenterY + 1f, crouchSpeed * Time.deltaTime),controller.center.z);
        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, originalHeight, Time.deltaTime * crouchSpeed);

            controller.center = new Vector3(controller.center.x, Mathf.MoveTowards(controller.center.y, originalCenterY, crouchSpeed * Time.deltaTime),controller.center.z);        
        }
    }

    void HandleInteract()
    {
        CheckForInteractable();
        // Poll interact button continuously
        bool interactInput = inputActions.Player.Interact.ReadValue<float>() > 0;

        // Only interact if cooldown passed
        if (currentInteractable != null && interactInput && Time.time - lastInteractTime >= interactCooldown)
        {
            currentInteractable.Interact();
            lastInteractTime = Time.time; // reset cooldown
        }
    }

    void HandleDrop()
    {
        bool dropInput = inputActions.Player.Drop.ReadValue<float>() > 0;

        // Only interact if cooldown passed
        if (dropInput)
        {
            holdController.DropItem();
        }
    }

    void HandleMovement()
    {
        // Stamina/sprint handler
        if (runInput && currentStamina > 0f)
        {
            isSprinting = true;
            regenTimer = 0f;
            currentStamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            isSprinting = false;
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay)
            {
                currentStamina += staminaRegen * Time.deltaTime;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Determine speed
        float speed = isSprinting ? runSpeed : walkSpeed;
        if (crouchInput) speed *= 0.5f; // slower if crouched

        // Move relative to player orientation
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // Jump
        if (jumpInput && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        jumpInput = false; // reset jump

        // Gravity
        if (!controller.isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0)
            velocity.y = -2f; // small downward force to keep grounded

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Rotate camera vertically
        rotationX -= lookInput.y * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Rotate player horizontally
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
    }


    void CheckForInteractable()
    {
        currentInteractable = null;
        if (interactPrompt != null) interactPrompt.SetActive(false);

        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

        Debug.DrawRay(r.origin, r.direction * InteractRange, Color.green); //Draw ray

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.TryGetComponent(out IInteractable interactObj))
            {
                currentInteractable = interactObj;
                
                interactText.text = $"{interactObj.InteractName} - [E] {interactObj.InteractPrompt}";
                if (interactPrompt != null) interactPrompt.SetActive(true);
            }
        }
    }
}
