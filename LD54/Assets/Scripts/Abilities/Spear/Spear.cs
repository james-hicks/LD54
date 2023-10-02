using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Spear : Ability
{
    int damage = 1;
    [SerializeField] private SpearHitbox spearHitbox;
    [SerializeField] private Animator animator;
    // Use the spear
    public override void execute()
    {
        animator.SetTrigger("Attack");
        Debug.Log("USE SPEAR");
        foreach (EnemyMovement enemy in spearHitbox.enemies)
        {
            enemy.HP -= damage + PlayerMovement.PlayerInstance.AP;

        }
    }
}
