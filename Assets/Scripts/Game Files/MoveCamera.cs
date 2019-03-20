using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = 0f;
    public float yMaxLimit = 90;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float keyZoomSpeed = 1f;
    public float keyRotateSpeed = 1f;

    private new Rigidbody rigidbody;

    private float x = 0.0f;
    private float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    void LateUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f,
            Input.GetAxis("Vertical"));
        target.transform.Translate(movement * distance * Time.deltaTime);

        if (Input.GetMouseButtonDown(2))
            HideCursor();
        if (Input.GetMouseButtonUp(2))
            ShowCursor();

        if (target)
        {
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            // Mouse drag
            if (Input.GetMouseButton(2))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                rotation = Quaternion.Euler(y, x, 0);
            }

            // Keyboard rotation
            x += Input.GetAxis("RotateCamLeftRight") * keyRotateSpeed * 0.02f;
            y -= Input.GetAxis("RotateCamUpDown") * keyRotateSpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            rotation = Quaternion.Euler(y, x, 0);


            // Zoom
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 10 * (distance / 10), distanceMin,
                distanceMax);
            distance += Input.GetAxis("ZoomCam") * keyZoomSpeed;
            yMinLimit = distance / 10;
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);

            Vector3 position = rotation * negDistance + target.position;

            target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, rotation.eulerAngles.y,
                target.transform.eulerAngles.z);

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
