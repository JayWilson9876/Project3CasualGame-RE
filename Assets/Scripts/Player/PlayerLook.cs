using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;     // reference to player’s body (to rotate horizontally)
    public Transform cameraHolder;   // reference to camera holder (to rotate vertically)

    private float xRotation = 0f;

    void Start()
    {
        // Lock and hide cursor for first-person control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (looking up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // prevents flipping over

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (turning body)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
