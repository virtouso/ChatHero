using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharactersScriptableObject",menuName ="Configurations/ Create Characters List",order=0)]

public class CharactersScriptableObject :ScriptableObject
{
  public List<CharacterModel> Characters;
}
