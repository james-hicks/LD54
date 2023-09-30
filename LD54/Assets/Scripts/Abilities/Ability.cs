using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Overiddable method to use an ability
    public virtual void execute()
    {
        throw new System.NotImplementedException();
    }
}
