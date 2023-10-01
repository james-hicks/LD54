using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearHitbox : MonoBehaviour
{
    public List<EnemyMovement> enemies = new List<EnemyMovement>();
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            enemies.Add(enemy);
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyMovement enemy))
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
        }
    }
}
