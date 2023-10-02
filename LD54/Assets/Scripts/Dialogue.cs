using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<string> KeyDialogues = new List<string>();
    public List<string> InteractDialogue = new List<string>();
}
