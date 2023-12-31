using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Ability
{
    public float offset;

    public GameObject projectile;
    public Transform shotPoint;
    [SerializeField] private Item ammo;
    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        base.Update();
    }

    public override void execute()
    {
        if (Inventory.PlayerInventory.GetItem(ammo) != null)
        {
            Instantiate(projectile, shotPoint.position, transform.rotation);
            Inventory.PlayerInventory.GetItem(ammo).ChangeQuantity(-1);
        }
        else
        {
#if UNITY_EDITOR
            Instantiate(projectile, shotPoint.position, transform.rotation);
            Debug.LogError("OUT OF AMMO");
#endif
        }


    }


}
