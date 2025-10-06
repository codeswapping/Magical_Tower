using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int Id;
    public int health;
    public int damage;
    public float attackTime;
    public float walkSpeed;
    public float attackSpeed;
}
