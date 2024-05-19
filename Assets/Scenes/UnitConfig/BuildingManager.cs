using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType { Wood, Stone, Gold }

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    List<Building> allBuildings = new List<Building>();
    public Building[] buildingPrefabs = default;
    public GameObject unitPrefab; 
    public int[] currentResources = default;

    [SerializeField] private ParticleSystem buildParticle;
    [SerializeField] private ParticleSystem finishParticle;
    [SerializeField] private float spawnRadius = 5f; 

    private BuildingUI ui;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentResources = new int[] { 0, 0, 0 }; 
        ui = FindObjectOfType<BuildingUI>();
        if (ui)
            ui.RefreshResources();
    }

    public void SpawnBuilding(int index, Vector3 position, float rotationAngle)
    {
        Building building = buildingPrefabs[index];
        if (!building.CanBuild(currentResources))
            return;

        building = Instantiate(buildingPrefabs[index], position, Quaternion.Euler(0f, rotationAngle, 0f));
        allBuildings.Add(building);
        building.attackable.onDestroy.AddListener(() => RemoveBuilding(building));

        int[] cost = building.Cost();
        for (int i = 0; i < cost.Length; i++)
        {
            currentResources[i] -= cost[i];
            RefreshResources();
        }
    }

    public void SpawnUnit(Vector3 position)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition(position);
        Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition(Vector3 center)
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        return new Vector3(center.x + randomOffset.x, center.y, center.z + randomOffset.y);
    }

    public List<Building> GetBuildings()
    {
        return allBuildings;
    }

    public Building GetPrefab(int index)
    {
        return buildingPrefabs[index];
    }

    public Building GetRandomBuilding()
    {
        if (allBuildings.Count > 0)
            return allBuildings[Random.Range(0, allBuildings.Count)];
        else
            return null;
    }

    public void RemoveBuilding(Building building)
    {
        allBuildings.Remove(building);
    }

    public void AddResource(ResourceType resourceType, int amount)
    {
        currentResources[(int)resourceType] += amount;
        RefreshResources();
    }

    public void RefreshResources()
    {
        if (ui != null)
        {
            ui.RefreshResources();
        }
    }

    public void ResetResources()
    {
        currentResources = new int[] { 0, 0, 0 };
        RefreshResources();
    }

    public void PlayParticle(Vector3 position)
    {
        if (buildParticle)
        {
            buildParticle.transform.position = position;
            buildParticle.Play();
        }
    }
}
