using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI goldText;

    private void Update()
    {
        int[] currentResources = BuildingManager.instance.currentResources;

        woodText.text = "WOOD: " + currentResources[(int)ResourceType.Wood];
        stoneText.text = "STONE: " + currentResources[(int)ResourceType.Stone];
        goldText.text = "GOLD: " + currentResources[(int)ResourceType.Gold];
    }
}