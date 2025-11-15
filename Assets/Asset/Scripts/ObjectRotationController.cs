using UnityEngine;
using UnityEngine.InputSystem;
using Vuforia;

public class ObjectRotationController : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 100f;
    public bool invertY = false;

    private ObjectInputActions inputActions;
    private Vector2 rotateInput;

    private bool markerVisible = false;

    private ImageTargetBehaviour imageTarget;

    void Awake()
    {
        inputActions = new ObjectInputActions();

        // cari ImageTarget ke atas (parent)
        imageTarget = GetComponentInParent<ImageTargetBehaviour>();

        if (imageTarget != null)
        {
            imageTarget.OnTargetStatusChanged += OnStatusChanged;
        }
    }

    void OnDestroy()
    {
        if (imageTarget != null)
        {
            imageTarget.OnTargetStatusChanged -= OnStatusChanged;
        }
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        markerVisible = status.Status == Status.TRACKED ||
                        status.Status == Status.EXTENDED_TRACKED;
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
        if (!markerVisible)
            return; // ❗ Hanya rotasi jika marker terlihat

        rotateInput = inputActions.Object.Rotate.ReadValue<Vector2>();

        float yaw = rotateInput.x * rotationSpeed * Time.deltaTime;
        float pitch = (invertY ? -rotateInput.y : rotateInput.y) * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, yaw, Space.Self);
        transform.Rotate(Vector3.right, pitch, Space.Self);
    }
}
