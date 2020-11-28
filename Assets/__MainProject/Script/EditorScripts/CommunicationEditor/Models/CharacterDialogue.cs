using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDialogue : Dialogue
{
    public string Dialogue;
    public int UserScore;// terminal state if value is greater than zero
    public string CharacterName;
}
