using UnityEngine;
using UnityEngine.UI;

public class UnitPurchaseUI : MonoBehaviour
{
    [SerializeField] private Button buyUnitButton;
    [SerializeField] private int unitCost = 30; 
    private Camera cam;

    private Barracks barracks;

    private void Start()
    {
        cam = Camera.main;
        buyUnitButton.onClick.AddListener(OnBuyUnitButtonClicked);
    }

    public void SetBarracks(Barracks barracks)
    {
        this.barracks = barracks;
    }

    private void OnBuyUnitButtonClicked()
    {
        if (BuildingManager.instance.currentResources[(int)ResourceType.Gold] >= unitCost)
        {
            BuildingManager.instance.currentResources[(int)ResourceType.Gold] -= unitCost;
            BuildingManager.instance.SpawnUnit(transform.position);
            BuildingManager.instance.RefreshResources();
            barracks.CloseUI();
        }
        else
        {
            Debug.Log("Not enough gold to buy the unit.");
            barracks.CloseUI();
        }
    }

    private void OnDestroy()
    {
        if (barracks != null)
        {
            barracks.CloseUI();
        }
    }
}
