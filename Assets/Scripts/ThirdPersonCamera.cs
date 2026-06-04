using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;

    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yRotation = player.eulerAngles.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(
            xRotation,
            -maxLookAngle,
            maxLookAngle
        );

        player.rotation =
            Quaternion.Euler(0f, yRotation, 0f);

        transform.rotation =
            Quaternion.Euler(xRotation, yRotation, 0f);
    }
}