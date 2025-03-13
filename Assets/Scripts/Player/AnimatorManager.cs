using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private Controller controller;
    int horizontal, vertical;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller>();
        playerMovement = GetComponent<PlayerMovement>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal, snappedVertical;

        if (horizontalMovement > 0 && horizontalMovement < .55f) snappedHorizontal = .5f;
        else if (horizontalMovement > .55f) snappedHorizontal = 1;
        else if (horizontalMovement < 0 && horizontalMovement > -.55f) snappedHorizontal = -.5f;
        else if (horizontalMovement < -.55f) snappedHorizontal = -1;
        else snappedHorizontal = 0;

        if (verticalMovement > 0 && verticalMovement < .55f) snappedVertical = .5f;
        else if (verticalMovement > .55f) snappedVertical = 1;
        else if (verticalMovement < 0 && verticalMovement > -.55f) snappedVertical = -.5f;
        else if (verticalMovement < -.55f) snappedVertical = -1;
        else snappedVertical = 0;

        if (isSprinting)
        {
            snappedVertical = 2;
            snappedHorizontal = horizontalMovement;
        }

        animator.SetFloat(horizontal, snappedHorizontal, .25f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, .25f, Time.deltaTime);
    }

    public void TargetAnimation(string animName, bool animBool, bool damagable = false)
    {
        animator.SetBool("IsInteracting", animBool);
        animator.CrossFade(animName, .2f);
    }
}
