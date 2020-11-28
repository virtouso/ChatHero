using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsReferencesScriptableObject", menuName = "Configurations/Create Levels References", order = 0)]
public class LevelsReferences : ScriptableObject
{

    public List<CommunicarionContainer> Levels;
}
