using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minFOV = 1f;
    public float maxFOV = 60f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 90f; 
    private Camera cameraToControl;

    private void Start()
    {
        cameraToControl = GetComponentInParent<Camera>();

        if (cameraToControl == null)
        {
            Debug.LogError("No camera component found in parent object.");
        }
    }

    private void Update()
    {
        if (cameraToControl == null)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 movement = (transform.right * horizontalInput + transform.forward * verticalInput) * moveSpeed * Time.deltaTime;
        movement.y = 0;
        transform.Translate(movement, Space.World);


        if (scrollInput != 0f)
        {
            float newFOV = cameraToControl.fieldOfView - scrollInput * zoomSpeed;

            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

            cameraToControl.fieldOfView = newFOV;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            RotateCamera(Vector3.up);
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotateCamera(Vector3.down);
        }
    }

    private void RotateCamera(Vector3 direction)
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(direction * rotationAmount, Space.World);
    }
}
