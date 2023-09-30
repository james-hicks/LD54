using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingShroom : Interactable
{
    [SerializeField] private Item _hpBoostItem;
    [SerializeField] private Item _apBoostItem;
    public override void OnInteract()
    {
        throw new System.NotImplementedException();

        // Talks to player telling the player what it needs next
        // Player can choose to give items when the player has what it needs
    }

    private void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(4, 4), 0);
        foreach(Collider2D col in hitColliders)
        {
            if(col.TryGetComponent(out ItemPickup newItem))
            {
                if (newItem.CanPickup)
                {
                    Debug.Log("Item Entered Trigger");
                    GiveItem(newItem.GetItem());
                    newItem.MoveToShroom(transform);
                }
            }
        }
    }

    public void GiveItem(Item newItem)
    {
        if(newItem == _hpBoostItem)
        {
            PlayerMovement.PlayerInstance.IncreaseMaxHealth();

        } else if (newItem == _apBoostItem)
        {
            PlayerMovement.PlayerInstance.ChangeAttackPower(1);
        }
    }
}
