using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = _wantedMode = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _wantedMode = CursorLockMode.Locked;
        }

        // Apply cursor state
        Cursor.lockState = _wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != _wantedMode);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCursorState();

        if (Cursor.visible)
            return;

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

        // set rigid body force
        _rigidBody.velocity = deltaPosition * _movementSpeed;

        
        // Pitch
        transform.rotation *= Quaternion.AngleAxis(
            -Input.GetAxis("Mouse Y") * _mouseSense,
            Vector3.right
        );

        // Paw
        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
            transform.eulerAngles.z
        );

        // set rigidbody rotation to the camera rotation
        _rigidBody.rotation = transform.rotation;
    }
}
