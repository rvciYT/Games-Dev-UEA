using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float minFOV = 1f;
    public float maxFOV = 60f;
    public float zoomSpeed = 5f;

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

        // Camera Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Camera Zoom
        if (scrollInput != 0f)
        {
            // Calculate new FOV
            float newFOV = cameraToControl.fieldOfView - scrollInput * zoomSpeed;

            // Clamp the FOV within minFOV and maxFOV
            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

            // Assign the new FOV
            cameraToControl.fieldOfView = newFOV;
        }
    }
}
