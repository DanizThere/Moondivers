using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IControllable
{
    public float movementSpeed;
    public float sprintSpeed;

    public float jumpForce;
    public float gravity = -9.81f;

    public float groundCheck = 1.5f;
    public LayerMask groundMask;

    private CharacterController characterController;
    private Controller controller;
    private float velocity;
    private Vector3 moveDirection;
    public bool isGrounded;
    [HideInInspector] public float currentSpeed;

    public float dodgeForce;
    public float dodgeTime;
    public float rotationSpeed;
    private Transform cameraObject;

    private AnimatorManager animatorManager;

    [HideInInspector] public bool isDodging = false;

    public float dashDownForce;
    public Transform checkUp;
    void Awake()
    {
        isGrounded = true;
        characterController = GetComponent<CharacterController>();
        controller = GetComponent<Controller>();
        animatorManager = GetComponent<AnimatorManager>();

        cameraObject = Camera.main.transform;
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheck, groundMask);
        if (isGrounded && velocity < 0)
        {
            velocity = -2;
        }

        MoveInternal();
        DoGravity();
        Debug.DrawRay(transform.up, transform.forward, Color.red, 1.5f);
    }


    public void Move(Vector3 dir)
    {
        moveDirection = dir;
    }


    private bool canJump = true;
    public void Jump()
    {
        if (isGrounded && canJump == true)
        {
            if (!controller.isInteractive)
            {
                animatorManager.TargetAnimation("Jump", !isGrounded);

                velocity = Mathf.Sqrt(jumpForce * -2 * gravity);
                canJump = false;
                Invoke(nameof(ResetJump), 2f);
            }           
        }

        if (!isGrounded && canClimb())
        {
            Debug.Log("Можешь залезать");
        }
    }

    bool canClimb()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1.5f) && !Physics.Raycast(checkUp.position, transform.forward, 1.5f))
        {
            return true;
        }
        return false;
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void MoveInternal()
    {
        characterController.Move(moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    private void DoGravity()
    {
        velocity += gravity * Time.fixedDeltaTime;

        characterController.Move(Vector3.up * velocity * Time.fixedDeltaTime);
    }

    public void PlayerRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        if (!isDodging || !controller.sprintInput)
        {
            targetDirection = cameraObject.forward;
        }
        if (isDodging || controller.sprintInput)
        {
            targetDirection = cameraObject.forward * controller.verticalInput;
            targetDirection += cameraObject.right * controller.horizontalInput;
        }
            
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            transform.rotation = playerRotation;       
    }

    public void Dodge()
    {
        if (!isGrounded) DashDown();
        if (controller.isInteractive) return;
        if (isGrounded)
        {
            DodgeInternal();            
        }        
    }


    private async void DodgeInternal()
    {
        float currentDodgeTime = dodgeTime;
        animatorManager.TargetAnimation("Dodging", isDodging);
        if (controller.moveAmount > .1f)
        {
            while (currentDodgeTime > 0)
            {
                currentDodgeTime -= Time.deltaTime;
                characterController.Move(controller.currentDirection.normalized * dodgeForce * Time.fixedDeltaTime);

                isDodging = true;
                await Task.Yield();
            }
            
        }
        if(controller.moveAmount < .1f)
        {        
            while (currentDodgeTime > 0)
            {
                currentDodgeTime -= Time.deltaTime;
                characterController.Move(transform.forward * dodgeForce * Time.fixedDeltaTime);

                isDodging = true;
                await Task.Yield();
            }
        }       
        isDodging = false;

        
    }

    public void Falling()
    {
        if (!isGrounded && !Physics.Raycast(transform.position, -transform.up, .7f, groundMask))
        {
            if (!controller.isInteractive)
            {
                animatorManager.TargetAnimation("Falling", !isGrounded);
            }
        }
        if (Physics.Raycast(transform.position, -transform.up, .7f, groundMask))
        {
            if (!isGrounded && controller.isInteractive)
            {
                animatorManager.TargetAnimation("Land", true);
            }
        }
        
    }

    private async void DashDown()
    {
        while (!isGrounded)
        {
            characterController.Move(-Vector3.up * dashDownForce * Time.fixedDeltaTime);
            await Task.Yield();
        }
    }
}
