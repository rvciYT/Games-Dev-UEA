using UnityEngine;
using System.Collections;

public class UnitClick : MonoBehaviour
{
    public LayerMask clickable;
    public LayerMask ground;
    private Camera mycam;
    public GameObject GroundMarker;
    private Coroutine markerDeactivationCoroutine;

    void Start()
    {
        mycam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mycam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelections.Instance.DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mycam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                GroundMarker.transform.position = hit.point;
                GroundMarker.SetActive(true);

                // Cancel the existing coroutine if it's running (so that each marker lasts specified time)
                if (markerDeactivationCoroutine != null)
                    StopCoroutine(markerDeactivationCoroutine);

                // Start a new coroutine
                markerDeactivationCoroutine = StartCoroutine(DeactivateGroundMarker());
            }
        }
    }

    IEnumerator DeactivateGroundMarker()
    {
        yield return new WaitForSeconds(1.5f);

        // Deactivate the ground marker
        if (GroundMarker.activeSelf)
            GroundMarker.SetActive(false);

        markerDeactivationCoroutine = null; // Reset the coroutine reference
    }
}
