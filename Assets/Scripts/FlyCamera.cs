using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Camera base movement speed")]
    private float _baseSpeed = 10f;

    [SerializeField]
    [Tooltip("Camera boost speed")]
    private float _boostSpeed = 0f;

    [SerializeField]
    [Tooltip("Camera move speed")]
    private float _moveSpeed = 10f;


    [Space]

    [SerializeField]
    [Tooltip("Sensitivity of mouse rotation")]
    private float _mouseSense = 1.8f;

    private Rigidbody _rigidBody;
    private CursorLockMode _wantedMode;

    private void SetCursorState()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            _wantedMode = CursorLockMode.None;

        if (Input.GetKeyDown(KeyCode.Mouse1))
            _wantedMode = CursorLockMode.Locked;

        // Apply cursor state
        Cursor.lockState = _wantedMode;
        // Hide cursor when locking
        Cursor.visible = (_wantedMode != CursorLockMode.Locked);
    }

    private void Boost()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            _moveSpeed = _boostSpeed;
        else
            _moveSpeed = _baseSpeed;
    }

    private void CameraMovement()
    {
        Vector3 deltaPosition = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            deltaPosition += transform.forward;

        if (Input.GetKey(KeyCode.S))
            deltaPosition -= transform.forward;

        if (Input.GetKey(KeyCode.A))
            deltaPosition -= transform.right;

        if (Input.GetKey(KeyCode.D))
            deltaPosition += transform.right;

        if (Input.GetKey(KeyCode.Q))
            deltaPosition -= transform.up;

        if (Input.GetKey(KeyCode.E))
            deltaPosition += transform.up;
        _rigidBody.velocity = _moveSpeed * Time.fixedUnscaledDeltaTime * deltaPosition;
    }

    private void CameraRotation()
    {
        if (_wantedMode == CursorLockMode.Locked)
        {
            // Pitch
            transform.rotation *= Quaternion.AngleAxis(
                -Input.GetAxis("Mouse Y") * _mouseSense,
                Vector3.right
            );

            // Yaw
            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                transform.eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
                transform.eulerAngles.z
            );
        }
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _boostSpeed = _baseSpeed * 3;
    }

    void Update()
    {
        SetCursorState();
        Boost();
        CameraMovement();
        CameraRotation();

    }
}
