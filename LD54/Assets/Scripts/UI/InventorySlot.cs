using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemQuantityText;
    [SerializeField] private Image _itemImage;
    private int _quantity;
    public Item Item;

    private void Update()
    {
        _itemQuantityText.text = _quantity.ToString();
    }

    public void Setup()
    {
        _itemImage.sprite = Item.Icon;
    }

    public void ChangeQuantity(int Quantity)
    {
        _quantity += Quantity;
        if(_quantity == 0)
        {
            Inventory.PlayerInventory.RemoveItem(Item);
        }
    }

    public void DropItem()
    {
        Debug.Log("Consumed the Item!");
        ChangeQuantity(-1);
        GameObject DroppedItem = Instantiate(Item.SpawnablePrefab, PlayerMovement.PlayerInstance.transform.position, Quaternion.identity);
        DroppedItem.GetComponent<ItemPickup>().SetItem(Item);
    }

}
