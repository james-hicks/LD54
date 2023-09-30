using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryRoot;
    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private List<InventorySlot> _inventorySlots = new List<InventorySlot>();

    private void Awake()
    {
        if (PlayerInventory != null)
        {
            Destroy(PlayerInventory);
        }
        else
        {
            PlayerInventory = this;
        }

        DontDestroyOnLoad(this);
    }


    public void PickupItem(Item Item, int Quantity)
    {
        bool hasItem = false;
        foreach (InventorySlot slot in _inventorySlots)
        {
            if (slot.Item == Item)
            {
                Debug.Log("Adding to Existing Item");
                hasItem = true;
                slot.ChangeQuantity(Quantity);
            }
        }

        if (!hasItem)
        {
            Debug.Log("Getting new Item");
            GameObject newSlot = Instantiate(_inventorySlotPrefab, _inventoryRoot.transform);
            newSlot.GetComponent<InventorySlot>().Item = Item;
            newSlot.GetComponent<InventorySlot>().ChangeQuantity(Quantity);
            newSlot.GetComponent<InventorySlot>().Setup();
            _inventorySlots.Add(newSlot.GetComponent<InventorySlot>());
        }
    }

    public static Inventory PlayerInventory;

}
