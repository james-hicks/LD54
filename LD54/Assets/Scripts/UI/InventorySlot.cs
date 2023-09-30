using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    }
}
