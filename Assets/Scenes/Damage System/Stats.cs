using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/UnitStats")]
public class Stats : ScriptableObject
{
    public float attackRange;
    public float attackSpeed;
    public float health;
    public float attackDamage;
}
