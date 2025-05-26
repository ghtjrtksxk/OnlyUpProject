using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
     [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    private CameraController _cameraController;

    private Rigidbody _rigidbody;
    private PlayerCondition _playerCondition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerCondition = GetComponent<PlayerCondition>();
        _cameraController = GetComponent<CameraController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void FixedUpdate()
    {
        Move();
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
            _cameraController.isMove = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            _cameraController.isMove = false;
        }
    }

    

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse); //점프; 빠르게 받는 물리값
        }
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
