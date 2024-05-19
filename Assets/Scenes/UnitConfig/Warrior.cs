using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Damageable))]
public class Warrior : Actor
{
    [SerializeField] private string enemyTag = "Enemy"; // Tag to identify enemies
    private float attackRange = 4f; // Range within which the warrior attacks enemies

    private void Start()
    {
        animationEvent.attackEvent.AddListener(Attack);
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20f, LayerMask.GetMask(enemyTag));
        foreach (Collider collider in hitColliders)
        {
            Damageable enemyDamageable = collider.GetComponent<Damageable>();
            if (enemyDamageable != null && CanAttackTarget(enemyDamageable))
            {
                AttackTarget(enemyDamageable);
                break;
            }
        }
    }

    private bool CanAttackTarget(Damageable target)
    {
        return Vector3.Distance(target.transform.position, transform.position) <= attackRange && currentTask == null;
    }

    private void AttackTarget(Damageable target)
    {
        StopTask();
        damageableTarget = target;

        currentTask = StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            while (damageableTarget)
            {
                SetDestination(damageableTarget.transform.position);
                yield return WaitForNavMesh();

                while (damageableTarget && Vector3.Distance(damageableTarget.transform.position, transform.position) < attackRange)
                {
                    yield return new WaitForSeconds(1);
                    if (damageableTarget)
                    {
                        animator.SetTrigger("Attack");
                        damageableTarget.Hit(10);
                    }
                }
            }

            currentTask = null;
        }
    }
}
