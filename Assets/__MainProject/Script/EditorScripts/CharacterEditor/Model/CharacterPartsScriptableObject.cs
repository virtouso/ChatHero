using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "CharacterPartsScriptableObject", menuName = "Configurations/Create Character Parts Lists", order = 0)]
public class CharacterPartsScriptableObject : ScriptableObject
{
    public List<Texture2D> FaceList;
    public List<Texture2D> ClothingList;
    public List<Texture2D> EyeList;
    public List<Texture2D> EyebrowList;
    public List<Texture2D> MouthList;
    public List<Texture2D> NoseList;
    public List<Texture2D> FacialList;

}
