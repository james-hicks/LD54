using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private Item item;
    private int ItemAmount = 1;
    public bool CanPickup = true;


    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = item.Icon;
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        GetComponent<SpriteRenderer>().sprite = item.Icon;
    }

    public Item GetItem()
    {
        return item;
    }

    public override void Update()
    {
        InteractUI.SetActive(PlayerInRange && CanPickup);
    }

    public override void OnInteract()
    {
        if (!CanPickup) return;
        Inventory.PlayerInventory.PickupItem(item, ItemAmount);
        Destroy(gameObject);
    }

    public void MoveToShroom(Transform shroomTransform)
    {
        StartCoroutine(MoveItemToShroom(shroomTransform));
        CanPickup = false;
    }

    private IEnumerator MoveItemToShroom(Transform shroomTransform)
    {
        while(Vector2.Distance(transform.position, shroomTransform.position) > 1)
        {
            transform.position = Vector2.Lerp(transform.position, shroomTransform.position, Time.deltaTime*2);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

}
