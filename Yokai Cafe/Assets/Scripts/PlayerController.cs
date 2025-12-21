using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    void Interact();
}
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpHeight = 0f;
    public float gravity = -9.81f;

    [Header("Crouch")]
    private float originalHeight;
    public float crouchHeight = 1f;
    public float crouchSpeed = 1.5f;



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
    public float InteractRange = 2f;

    [Header("UI")]
    public GameObject interactPrompt;
    private IInteractable currentInteractable;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Crouching
        originalHeight = controller.height;

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
    }
    void HandleCrouch()
    {
        if (crouchInput)
        {
            controller.height = Mathf.Lerp( controller.height, crouchHeight, Time.deltaTime * crouchSpeed );
            
        } else
        {
            controller.height = Mathf.Lerp( controller.height, originalHeight, Time.deltaTime * crouchSpeed );
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

    void HandleMovement()
    {
        // Determine speed
        float speed = runInput ? runSpeed : walkSpeed;
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
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.TryGetComponent(out IInteractable interactObj))
            {
                currentInteractable = interactObj;
                if (interactPrompt != null) interactPrompt.SetActive(true);
            }
        }
    }
}
