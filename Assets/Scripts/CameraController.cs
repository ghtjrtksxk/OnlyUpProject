using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Look")]
    public Transform _Character;
    private float minXLook;
    private float maxXLook;
    private float camCurXRot;
    private float lookSensitivity;
    private Vector2 mouseDelta;
    public bool isMove;

    // Update is called once per frame
    void Update()
    {
        CameraLook();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        if (isMove)
        {
            maxXLook = 85.0f;
            _Character.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
            transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        }
        else
        {
            maxXLook = 50.0f;
            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);

        }
        transform.localEulerAngles = new Vector3(-camCurXRot, transform.eulerAngles.y - _Character.eulerAngles.y, 0);
    }
}
