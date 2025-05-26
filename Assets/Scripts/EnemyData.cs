
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]

// scriptable object holds some data for us that we can change
public class EnemyData : ScriptableObject
{
    public int hp;
    public int damage;
    public float speed;
}
