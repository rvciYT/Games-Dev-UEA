using UnityEngine;

public class Barracks : MonoBehaviour
{
    [SerializeField] private GameObject unitPurchaseUI;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        if (unitPurchaseUI != null)
        {
            unitPurchaseUI.SetActive(false);
        }
        else
        {
            Debug.LogError("UnitPurchaseUI is not assigned.");
        }
    }

    private void OnMouseDown()
    {
        if (unitPurchaseUI != null)
        {
            unitPurchaseUI.SetActive(true);


            UnitPurchaseUI unitPurchaseUIScript = unitPurchaseUI.GetComponent<UnitPurchaseUI>();
            if (unitPurchaseUIScript != null)
            {
                unitPurchaseUIScript.SetBarracks(this);
            }
            else
            {
                Debug.LogError("UnitPurchaseUI component is not found in the child object.");
            }
        }
    }

    public void CloseUI()
    {
        if (unitPurchaseUI != null)
        {
            unitPurchaseUI.SetActive(false);
        }
    }
}
