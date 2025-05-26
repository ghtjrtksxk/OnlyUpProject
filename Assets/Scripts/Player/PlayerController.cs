using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
     [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    private float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    private bool isMove;

    private Rigidbody _rigidbody;
    private PlayerCondition _playerCondition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerCondition = GetComponent<PlayerCondition>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            isMove = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            isMove = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse); //점프; 빠르게 받는 물리값
        }
    }
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        if (isMove)
        {
            maxXLook = 85.0f;
            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
            cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        }
        else
        {
            maxXLook = 50.0f;
            cameraContainer.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);

        }
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, cameraContainer.eulerAngles.y - transform.eulerAngles.y, 0);
    }
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up *0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up *0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up *0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up *0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    public void ItemEffect(ItemData data)
    {
        if (data.displayName == "체력회복의 돌")
        {
            _playerCondition.Heal(10.0f);
        }
        else if (data.displayName == "스피드의 돌")
        {
            StartCoroutine(MultiplyMoveSpeed());
        }
    }
    private IEnumerator MultiplyMoveSpeed()
    {
        moveSpeed *= 2;
        yield return new WaitForSeconds(4f);
        moveSpeed /= 2;
    }
}
