using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public float topClamp = 90f;
    public float bottomClamp = -90f; // Corrected bottom clamp

    void Start()
    {
        // Lock the cursor to the middle of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Getting the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation around the X-axis (Looking up and down)
        xRotation -= mouseY; // Inverted to match standard FPS movement
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp); // Corrected order

        // Rotation around the Y-axis (Looking left and right)
        yRotation += mouseX;

        // Apply rotations to the transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}




