using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    bool isPlacing = false;
    int currentIndex = 0;
    float RotationAngle = 0;

    public Transform resourceGroup;

    [SerializeField] const float rotationAngle = 30f;
    GameObject buildingPreviewObject; // Instantiated building preview object

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[index].onClick.AddListener(() => SelectBuilding(index));

            Building b = BuildingManager.instance.buildingPrefabs[index];
            buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = GetButtonText(b);
        }
    }

    private void Update()
    {
        if (isPlacing)
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateBuilding();
                Debug.Log(RotationAngle);
            }

            Vector3 position = Utility.MouseToTerrainPosition();
            if (buildingPreviewObject != null)
                buildingPreviewObject.transform.position = position;
            if (Input.GetMouseButtonDown(0))
            {
                BuildingManager.instance.SpawnBuilding(currentIndex, position, RotationAngle);
                Destroy(buildingPreviewObject); // Destroy the preview object
                canvasGroup.alpha = 1;
                isPlacing = false;
                RotationAngle = 0f;
            }
        }
    }

    void RotateBuilding()
    {
        if (buildingPreviewObject != null)
            RotationAngle += 30f;
            buildingPreviewObject.transform.Rotate(Vector3.up, rotationAngle);
    }

    void SelectBuilding(int index)
    {
        currentIndex = index;
        ActorManager.instance.DeselectActors();
        canvasGroup.alpha = 0;
        isPlacing = true;

        // Instantiate the selected building prefab for preview
        Building selectedBuilding = BuildingManager.instance.buildingPrefabs[currentIndex];
        if (selectedBuilding != null)
        {
            Destroy(buildingPreviewObject); // Destroy previous preview
            buildingPreviewObject = Instantiate(selectedBuilding.gameObject, Vector3.zero, Quaternion.identity);
        }
    }

    string GetButtonText(Building b)
    {
        string buildingName = b.buildingName;
        int resourceAmount = b.resourceCost.Length;
        string[] resourceNames = new string[] { "Wood", "Stone" };
        string resourceString = string.Empty;
        for (int j = 0; j < resourceAmount; j++)
            resourceString += "\n " + resourceNames[j] + " (" + b.resourceCost[j] + ")";

        return "<size=30><b>" + buildingName + "</b></size>" + resourceString;
    }

    public void RefreshResources()
    {
        for (int i = 0; i < resourceGroup.childCount; i++)
            resourceGroup.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = BuildingManager.instance.currentResources[i].ToString();
    }
}
