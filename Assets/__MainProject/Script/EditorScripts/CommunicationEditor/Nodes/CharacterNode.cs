using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterNode : DialogueNode
{
    public string CharacterName;
    public int UserScore;//possible values 0-3 if Userscore if greater than zero, its a terminal state and you can show result to the user
}
