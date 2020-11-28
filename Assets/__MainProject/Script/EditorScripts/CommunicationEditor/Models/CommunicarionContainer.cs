using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicarionContainer : ScriptableObject
{
    public List<PlayerDialogue> PlayerDialogues = new List<PlayerDialogue>();
    public List<CharacterDialogue> CharacterDialogues = new List<CharacterDialogue>();
    public List<EdgeModel> Edges = new List<EdgeModel>();
}
