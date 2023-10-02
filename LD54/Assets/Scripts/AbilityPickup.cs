using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : Interactable
{
    [SerializeField] private Item item;
    [SerializeField] private GameObject _bowPrefab;
    public bool CanPickup = true;
    public override void OnInteract()
    {
        if (!CanPickup) return;
        GameObject newBow = Instantiate(_bowPrefab, PlayerMovement.PlayerInstance.transform);
        newBow.transform.position = new Vector3(PlayerMovement.PlayerInstance.transform.position.x + 0.084f, PlayerMovement.PlayerInstance.transform.position.y - 0.438f, PlayerMovement.PlayerInstance.transform.position.z);
        newBow.GetComponent<Bow>().shotPoint = newBow.transform;
        PlayerMovement.PlayerInstance.GetComponent<PlayerWeapon>().bow = newBow.GetComponent<Bow>();
        Destroy(gameObject);
    }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = item.Icon;
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        GetComponent<SpriteRenderer>().sprite = item.Icon;
    }

    public override void Update()
    {
        InteractUI.SetActive(PlayerInRange && CanPickup);
    }

}
