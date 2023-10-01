using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Spear : Ability
{
    int damage = 1;
    [SerializeField] private SpearHitbox spearHitbox;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Use the spear
    public override void execute()
    {
        Debug.Log("USE SPEAR");
        foreach (EnemyMovement enemy in spearHitbox.enemies)
        {
            enemy.HP -= damage;
        }
    }
}
