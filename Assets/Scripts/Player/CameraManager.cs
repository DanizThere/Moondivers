using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Controller controller;
    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public Transform aimPosition;

    public LayerMask collisionLayers;

    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    private Camera mainCam;
    public Camera aimCam;

    public Transform origPivotPos;
    public Transform aimPivotPos;

    public float followSpeed;
    public float cameraCollisionRadius;
    public float lookAngle;
    public float pivotAngle;
    public float minCollisionOffset;

    public float lookSpeed, pivotSpeed;
    public float minPivotAngleMain, maxPivotAngleMain;
    public float minPivotAngleAim, maxPivotAngleAim;
    private float minPivotAngle, maxPivotAngle;
    public float cameraCollisionOffset;
    public float camLookSmoothTime = 1;
    private void Awake()
    {
        mainCam = GetComponent<Camera>();
        mainCam = Camera.main;
        controller = FindObjectOfType<Controller>();
        targetTransform = FindObjectOfType<Controller>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        HandleAllCameraMovement();
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
        HandleCameraPivot();
    }

    private void HandleCameraPivot()
    {
        if (controller.isAiming == true)
        {
            aimCam.enabled = true;
            mainCam.enabled = false;
            aimCam = Camera.main;
            minPivotAngle = minPivotAngleAim;
            maxPivotAngle = maxPivotAngleAim;
        }
        else
        {
            aimCam.enabled = false;
            mainCam.enabled = true;
            mainCam = Camera.main;
            minPivotAngle = minPivotAngleMain;
            maxPivotAngle = maxPivotAngleMain;
        }
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, followSpeed * Time.deltaTime); 
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle += (controller.cameraX * lookSpeed * Time.deltaTime);
        pivotAngle -= (controller.cameraY * pivotSpeed * Time.deltaTime);

        lookAngle = Mathf.Lerp(lookAngle, lookAngle + (controller.cameraX * lookSpeed), camLookSmoothTime * Time.deltaTime);
        pivotAngle = Mathf.Lerp(pivotAngle, pivotAngle - (controller.cameraY * pivotSpeed), camLookSmoothTime * Time.deltaTime);

        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition -= (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset) targetPosition -= minCollisionOffset;

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, .2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }

    private void HandleCameraShake()
    {

    }

}
