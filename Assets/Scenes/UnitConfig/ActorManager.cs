using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class ActorManager : MonoBehaviour
{
    public static ActorManager instance;
    [SerializeField] LayerMask actorLayer = default;
    [SerializeField] Transform selectionArea = default;
    public List<Actor> allActors = new List<Actor>();
    [SerializeField] List<Actor> selectedActors = new List<Actor>();
    Camera mainCamera;
    Vector3 startDrag;
    Vector3 endDrag;
    Vector3 dragCenter;
    Vector3 dragSize;
    bool dragging;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;
        foreach (Actor actor in GetComponentsInChildren<Actor>())
        {
            allActors.Add(actor);
        }

        selectionArea.gameObject.SetActive(false);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            startDrag = Utility.MouseToTerrainPosition();
            endDrag = startDrag;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Collider collider = Utility.CameraRay().collider;
            if (!collider.CompareTag("Player"))
            {
               SetTask(); 
            }
            else
            {
                if(!Input.GetKey(KeyCode.LeftShift))
                {
                    SelectIndividualActor();
                }
                else
                {
                    AddToSelection();
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            endDrag = Utility.MouseToTerrainPosition();

            if (Vector3.Distance(startDrag, endDrag) > 1)
            {
                selectionArea.gameObject.SetActive(true);
                dragging = true;
                dragCenter = (startDrag + endDrag) / 2;
                dragSize = (endDrag - startDrag);
                selectionArea.transform.position = dragCenter;
                selectionArea.transform.localScale = dragSize + Vector3.up;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (dragging)
            {
                SelectActors();
                dragging = false;
                selectionArea.gameObject.SetActive(false);
            }
        }

    }

    void SetTask()
    {
        if (selectedActors.Count == 0)
            return;
        Collider collider = Utility.CameraRay().collider;
        if (collider.CompareTag("Terrain"))
        {
            foreach (Actor actor in selectedActors)
            {
                actor.SetDestination(Utility.MouseToTerrainPosition());
            }
        }
        else if (!collider.CompareTag("Player"))
        {
            if (collider.TryGetComponent(out Damageable damageable))
            {
                foreach (Actor actor in selectedActors)
                {
                    actor.AttackTarget(damageable);
                    Debug.Log(damageable);
                }
            }
        }


    }

    void SelectIndividualActor()
    {
        RaycastHit hitInfo;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, actorLayer.value))
        {
            Actor actor = hitInfo.collider.GetComponent<Actor>();
            if (actor != null)
            {
                if (selectedActors.Contains(actor))
                {
                    actor.visualHandler.Deselect();
                    selectedActors.Remove(actor);
                }
                else
                {
                    DeselectActors();
                    actor.visualHandler.Select();
                    selectedActors.Add(actor);
                }
            }
        }
        else
        {
            DeselectActors();
        }
    }

    void AddToSelection()
    {
        RaycastHit hitInfo;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, actorLayer.value))
        {
            Actor actor = hitInfo.collider.GetComponent<Actor>();
            if (actor != null && !selectedActors.Contains(actor))
            {
                actor.visualHandler.Select();
                selectedActors.Add(actor);
            }
        }
    }

    void SelectActors()
    {
        DeselectActors();
        dragSize.Set(Mathf.Abs(dragSize.x / 2), 1, Mathf.Abs(dragSize.z / 2));
        RaycastHit[] hits = Physics.BoxCastAll(dragCenter, dragSize, Vector3.up, Quaternion.identity, 0, actorLayer.value);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out Actor actor))
            {
                selectedActors.Add(actor);
                actor.visualHandler.Select();
            }
        }
    }
    public void DeselectActors()
    {
        foreach (Actor actor in selectedActors)
            actor.visualHandler.Deselect();

        selectedActors.Clear();
    }

    private void OnDrawGizmos()
    {
        Vector3 center = (startDrag + endDrag) / 2;
        Vector3 size = (endDrag - startDrag);
        size.y = 1;
        Gizmos.DrawWireCube(center, size);
    }
}
