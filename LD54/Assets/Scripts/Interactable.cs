using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public GameObject InteractUI;
    public bool PlayerInRange = false;
    public abstract void OnInteract();


    public virtual void Update()
    {
        InteractUI.SetActive(PlayerInRange);
    }
}
