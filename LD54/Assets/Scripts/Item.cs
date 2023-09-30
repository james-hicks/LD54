using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public GameObject SpawnablePrefab;
}
