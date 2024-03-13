using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class UnitMovement : MonoBehaviour
{
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Damageable damageableTarget;
    [HideInInspector] public AnimationEventListener animationEvent;
    [HideInInspector] public Coroutine currentTask;
    [HideInInspector] public ActorVisualHandler visualHandler;
    [HideInInspector] public Animator animator;
    NavMeshAgent myAgent;

    bool isResource;

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        visualHandler = GetComponent<ActorVisualHandler>();
        isResource = GetComponent<Resource>() ? true : false;
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if(IsUnitSelected())
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    GameObject targetObject = hit.transform.gameObject;
                    Resource resource = targetObject.GetComponent<Resource>();

                    if (resource != null)
                    {
                        AttackTarget(targetObject);
                    }
                    else
                    {
                        myAgent.SetDestination(hit.point);
                        StopTask();
                    }
                }
            }
            UpdateAnimation();
        }
    }

    bool IsUnitSelected()
    {
        return UnitSelections.Instance.unitsSelected.Contains(gameObject);
    }


    void UpdateAnimation()
    {
        float speed = myAgent.velocity.magnitude / myAgent.speed;
        //Debug.Log(speed);
        animator.SetFloat("Blend", speed);
    }

    ////////////////////////////////

      void Attack()
    {
        if (damageableTarget)
        {
            damageableTarget.Hit(10);
        }  
    }
    
    public void AttackTarget(GameObject targetObject)
    {
        // Get the Damageable component from the target GameObject
        Damageable targetDamageable = targetObject.GetComponent<Damageable>();
        
        if (targetDamageable != null)
        {
            // Start moving towards the target
            myAgent.SetDestination(targetObject.transform.position);

            currentTask = StartCoroutine(StartAttack(targetDamageable));
        }
    }

    IEnumerator StartAttack(Damageable targetDamageable)
    {
        // Move towards the target until within attack range
        while (Vector3.Distance(targetDamageable.transform.position, transform.position) > 4f)
        {
            // Continue moving towards the target while it's not in range
            myAgent.SetDestination(targetDamageable.transform.position);
            yield return null;
        }

        if (targetDamageable == null)
        
        {
        // Target has been destroyed, stop attacking
        currentTask = null;
        yield break;
        }

        // Stop moving and play attack animation
        myAgent.ResetPath();

        // Continuously attack the target until its health reaches zero
        while (targetDamageable && targetDamageable.currentHealth > 0)
        {
            animator.SetTrigger("AttackTrigger");
            yield return new WaitForSeconds((float)1.2);
            
            Debug.Log("Hit!");
            // Deal damage to the target
            targetDamageable.Hit(10);

            // Wait for a short duration before the next attack
            yield return new WaitForSeconds(1);
        }

        currentTask = null;
    }




    public virtual void StopTask()
    {
        damageableTarget = null;
        if (currentTask != null)
            StopCoroutine(currentTask);
            currentTask = null;
    }

}

