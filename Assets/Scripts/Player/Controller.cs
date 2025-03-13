using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerMovement playerMovement;
    private IControllable controllable;
    private AnimatorManager animatorManager;
    private Animator animator;
    [HideInInspector] public bool sprintInput;

    public bool isInteractive;

    [HideInInspector] public bool isAiming;
    [HideInInspector] public float verticalInput, horizontalInput, cameraX, cameraY;
    [HideInInspector] public float moveAmount;
    Vector2 cameraInput;
    public Vector2 movementInput;
    Transform cameraObject;
    private void Awake()
    {
        playerController = new PlayerController();
        playerController.Enable();

        playerMovement = GetComponent<PlayerMovement>();
        controllable = GetComponent<IControllable>();
        animatorManager = GetComponent<AnimatorManager>();

        cameraObject = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerController.Movement.Keybinds.performed += i => movementInput = i.ReadValue<Vector2>();

        playerController.Movement.Jump.performed += OnJumpPerformed;
        playerController.Movement.Dodge.performed += OnDodgePerformed;
        playerController.Movement.Sprint.performed += i => sprintInput = true;
        playerController.Movement.Sprint.canceled += i => sprintInput = false;
        playerController.Movement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

        playerController.Movement.WeaponInput.performed += i => isAiming = true;
        playerController.Movement.WeaponInput.canceled += i => isAiming = false;
    }

    private void OnDisable()
    {
        playerController.Movement.Jump.performed -= OnJumpPerformed;
        playerController.Movement.Dodge.performed -= OnDodgePerformed;
    }
    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        controllable.Jump();
    }
    private void OnDodgePerformed(InputAction.CallbackContext obj)
    {
        controllable.Dodge();
    }
    private void Update()
    {
        playerMovement.PlayerRotation();
    }

    private void FixedUpdate()
    {       
        UpdateInput();       
        if (sprintInput) { playerMovement.currentSpeed = playerMovement.sprintSpeed; }
        else playerMovement.currentSpeed = playerMovement.movementSpeed;

        isInteractive = animator.GetBool("IsInteracting");
        playerMovement.Falling();
    }
    public Vector3 currentDirection;
    private void UpdateInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraX = cameraInput.x;
        cameraY = cameraInput.y;

        currentDirection = cameraObject.forward * verticalInput + cameraObject.right * horizontalInput;
        currentDirection.Normalize();
        controllable.Move(currentDirection);

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        animatorManager.UpdateAnimatorValues(0, moveAmount, sprintInput);
    }
}
