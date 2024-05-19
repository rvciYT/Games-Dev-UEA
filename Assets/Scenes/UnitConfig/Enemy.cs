using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Damageable))]
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Animator animator;
    [SerializeField] private string buildingTag = "TownHouse"; // Tag to identify the building to attack
    [SerializeField] private float spreadRadius = 10f; // Radius for spreading out the enemies

    private NavMeshAgent agent;
    private Damageable damageableTarget;
    private Coroutine currentTask;
    private AnimationEventListener animationEvent;
    private ActorVisualHandler visualHandler;

    private void Start()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animationEvent = GetComponentInChildren<AnimationEventListener>();
        visualHandler = GetComponent<ActorVisualHandler>();
        animationEvent.attackEvent.AddListener(Attack);

        if (damageable == null || animator == null || agent == null)
        {
            Debug.LogError("Required components are missing on the Enemy!");
        }

        FindBuildingToAttack();
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Clamp(agent.velocity.magnitude, 0, 1));
    }

    private void SetDestination(Vector3 destination)
    {
        Vector3 randomOffset = Random.insideUnitSphere * spreadRadius;
        randomOffset.y = 0; // Keep the offset on the horizontal plane
        Vector3 targetPosition = destination + randomOffset;
        agent.destination = targetPosition;
        Debug.Log($"Setting destination to {targetPosition}");
    }

    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }

    private void FindBuildingToAttack()
    {
        GameObject building = GameObject.FindWithTag(buildingTag);
        if (building != null)
        {
            damageableTarget = building.GetComponent<Damageable>();
            if (damageableTarget != null)
            {
                AttackTarget(damageableTarget);
            }
            else
            {
                Debug.LogError("Building with tag " + buildingTag + " does not have a Damageable component!");
            }
        }
        else
        {
            Debug.LogError("No building found with tag " + buildingTag + "!");
        }
    }

    private void AttackTarget(Damageable target)
    {
        StopTask();
        Debug.Log("Triggering attack ");
        damageableTarget = target;

        currentTask = StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        while (damageableTarget != null)
        {
            SetDestination(damageableTarget.transform.position);
            Debug.Log("Setting Destination");
            yield return WaitForNavMesh();

            while (damageableTarget != null && Vector3.Distance(damageableTarget.transform.position, transform.position) < 10f)
            {
                if (CheckForObstacles())
                {
                    yield return AttackObstacle();
                }
                else
                {
                    yield return new WaitForSeconds(1);
                    if (damageableTarget != null)
                    {
                        Debug.Log("Triggering attack animation");
                        animator.SetTrigger("Attack");
                        damageableTarget.Hit(10);
                    }
                }
            }
        }

        currentTask = null;
    }

    private bool CheckForObstacles()
    {
        Ray ray = new Ray(transform.position, agent.steeringTarget - transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, agent.stoppingDistance))
        {
            Damageable obstacle = hit.collider.GetComponent<Damageable>();
            if (obstacle != null && obstacle != damageableTarget)
            {
                damageableTarget = obstacle;
                return true;
            }
        }

        return false;
    }

    private IEnumerator AttackObstacle()
    {
        while (damageableTarget != null && Vector3.Distance(damageableTarget.transform.position, transform.position) < 4f)
        {
            yield return new WaitForSeconds(1);
            if (damageableTarget != null)
            {
                Debug.Log("Attacking obstacle");
                animator.SetTrigger("Attack");
                damageableTarget.Hit(5);
            }
        }

        FindBuildingToAttack(); // Return to original target
    }

    private void StopTask()
    {
        damageableTarget = null;
        if (currentTask != null)
        {
            StopCoroutine(currentTask);
        }
    }

    private void Attack()
    {
        if (damageableTarget != null)
        {
            damageableTarget.Hit(5);
        }
    }
}
