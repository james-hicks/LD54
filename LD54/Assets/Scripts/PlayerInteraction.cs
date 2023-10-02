using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> _nearbyItems = new List<Interactable>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactable item))
        {
            _nearbyItems.Add(item);
            item.PlayerInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactable item))
        {
            if(_nearbyItems.Contains(item))
            {
                _nearbyItems.Remove(item);
                item.PlayerInRange = false;
            }
        }
    }

    public void InteractWithObject()
    {
        if (_nearbyItems[0] != null)
        {
            Interactable interactable = _nearbyItems[0];
            _nearbyItems.Remove(_nearbyItems[0]);
            interactable.OnInteract();
            
        }
    }
}
