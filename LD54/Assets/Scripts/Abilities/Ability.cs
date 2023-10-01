using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public float cooldown = 2f;
    private float currentCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        currentCooldown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentCooldown >= cooldown)
        {
            execute();
            currentCooldown = 0;
        }
    }

    // Overiddable method to use an ability
    public virtual void execute()
    {
        throw new System.NotImplementedException();
    }
}
