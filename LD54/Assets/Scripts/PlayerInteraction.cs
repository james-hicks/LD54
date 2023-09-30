using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Interactable _nearbyItem;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Item Enter");
        if (collision.TryGetComponent(out Interactable item))
        {
            _nearbyItem = item;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Item Exit");
        if (collision.TryGetComponent(out Interactable item))
        {
            if(item ==  _nearbyItem)
            {
                _nearbyItem = null;
            }
        }
    }

    public void InteractWithObject()
    {
        if(_nearbyItem != null) _nearbyItem.OnInteract();
    }
}
