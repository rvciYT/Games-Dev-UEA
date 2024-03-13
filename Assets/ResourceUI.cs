using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;

    private void Update()
    {
        // Get the current resource quantities from the BuildingManager
        int[] currentResources = BuildingManager.instance.currentResources;

        // Update the UI Text elements with the current resource quantities
        woodText.text = "Wood: " + currentResources[(int)ResourceType.Wood];
        stoneText.text = "Stone: " + currentResources[(int)ResourceType.Stone];
    }
}