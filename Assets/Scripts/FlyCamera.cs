using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Camera movement speed")]
    private float _movementSpeed = 10f;

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

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.timeScale == 1) // game is not paused
        {
            SetCursorState();

            // camera movement
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

            // apply deltaPosition as rigidBody velocity
            _rigidBody.velocity = _movementSpeed * Time.deltaTime * deltaPosition;

            // camera rotation - only when right mouse button is pressed
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
    }
}
