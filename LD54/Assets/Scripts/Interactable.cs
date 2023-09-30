using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnInteract();
        }
    }

    public abstract void OnInteract();
}
