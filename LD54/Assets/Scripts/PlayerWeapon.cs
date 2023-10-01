using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Spear spear;
    public Bow bow;
    public Ability weapon;

    public float cooldown = 2f;
    private float currentCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        weapon = spear;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch between weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = spear;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = bow;
        }
        spear.gameObject.SetActive(weapon == spear);
        bow.gameObject.SetActive(weapon == bow);

        // Use Weapons
        currentCooldown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentCooldown >= cooldown)
        {
            weapon.execute();
            currentCooldown = 0;
        }

    }
}
