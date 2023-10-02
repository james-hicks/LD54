using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable _nearbyItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactable item))
        {
            _nearbyItem = item;
            item.PlayerInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactable item))
        {
            if(item ==  _nearbyItem)
            {
                _nearbyItem = null;
                item.PlayerInRange = false;
            }
        }
    }

    public void InteractWithObject()
    {
        if(_nearbyItem != null) _nearbyItem.OnInteract();
    }
}
