using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private Item item;
    private int ItemAmount = 1;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = item.Icon;
    }

    public override void OnInteract()
    {
        Inventory.PlayerInventory.PickupItem(item, ItemAmount);
        Destroy(gameObject);
    }

}
