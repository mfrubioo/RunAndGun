using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    [Header("Camera")]
    Transform cameraPivot;
    public float mouseSensitivity = 20f;
    float smoothTime = 0.05f;

    PlayerInputActions inputActions;

    Vector2 moveInput;
    Vector2 lookInput;

    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    float xRotation = 0f;

    void Awake()
    {
        SetUpInputActions();
        SetUpCamera();
    }

    void SetUpInputActions()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void SetUpCamera()
    {
        cameraPivot = GameObject.Find("Personaje").GetComponentInChildren<Camera>().gameObject.transform;
    }

    void OnEnable()
    {
        inputActions.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        transform.position += move * speed * Time.deltaTime;
    }

    void Look()
    {
        Vector2 targetMouseDelta = lookInput * mouseSensitivity * Time.deltaTime;

        currentMouseDelta = Vector2.SmoothDamp(
            currentMouseDelta,
            targetMouseDelta,
            ref currentMouseDeltaVelocity,
            smoothTime
        );

        float mouseX = currentMouseDelta.x;
        float mouseY = currentMouseDelta.y;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}