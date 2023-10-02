using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public int damage;

    void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered collision with " + collision.gameObject.name);
        if (collision.TryGetComponent(out EnemyMovement enemy))
        {
            enemy.HP -= damage + PlayerMovement.PlayerInstance.AP;
            DestroyProjectile();
        }
        if (collision.TryGetComponent(out ElderTree tree))
        {
            tree.HP -= damage + PlayerMovement.PlayerInstance.AP;
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
