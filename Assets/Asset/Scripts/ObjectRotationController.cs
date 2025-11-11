using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRotationController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;
    public bool invertY = false;

    private ObjectInputActions inputActions;
    private Vector2 rotateInput;

    void Awake()
    {
        inputActions = new ObjectInputActions();
    }

    void OnEnable()
    {
        inputActions.Object.Enable();
    }

    void OnDisable()
    {
        inputActions.Object.Disable();
    }

    void Update()
    {
        rotateInput = inputActions.Object.Rotate.ReadValue<Vector2>();

        // rotasi kiri-kanan di sumbu Y
        float yaw = rotateInput.x * rotationSpeed * Time.deltaTime;
        // rotasi atas-bawah di sumbu X
        float pitch = (invertY ? -rotateInput.y : rotateInput.y) * rotationSpeed * Time.deltaTime;

        // Terapkan rotasi ke objek 3D
        transform.Rotate(Vector3.up, yaw, Space.World);
        transform.Rotate(Vector3.right, pitch, Space.World);
    }
}
