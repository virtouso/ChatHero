using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CharacterAssetsUpdator : Editor
{

    [MenuItem("ChatHero/ Update Character Assets")]
    public static void UpdateCharacterAssets()
    {

        UpdateAssets();
    }



    private const string _characterPartsScriptableObjectAddress = "Assets/__MainProject/Configuration/Character Parts/CharacterPartsScriptableObject.asset";
    private const string _basePartsSubUrl = "/__MainProject/Content/Image/UI/Character/";
    private static CharacterPartsScriptableObject _characterPartsReference;



    private static void UpdateAssets()
    {

        _characterPartsReference = (CharacterPartsScriptableObject)AssetDatabase.LoadAssetAtPath(_characterPartsScriptableObjectAddress, typeof(CharacterPartsScriptableObject));


        _characterPartsReference.FaceList = UpdateCharacterReference("Face", "f", "Assets" + _basePartsSubUrl);
        _characterPartsReference.ClothingList = UpdateCharacterReference("Clothing", "cl", "Assets" + _basePartsSubUrl);
        _characterPartsReference.EyeList = UpdateCharacterReference("Eye", "ey", "Assets" + _basePartsSubUrl);
        _characterPartsReference.EyebrowList = UpdateCharacterReference("Eyebrow", "eb", "Assets" + _basePartsSubUrl);
        _characterPartsReference.MouthList = UpdateCharacterReference("Mouth", "m", "Assets" + _basePartsSubUrl);
        _characterPartsReference.EarList = UpdateCharacterReference("Ear", "e", "Assets" + _basePartsSubUrl);
        _characterPartsReference.EaringList = UpdateCharacterReference("Earing", "er", "Assets" + _basePartsSubUrl);
        _characterPartsReference.NoseList = UpdateCharacterReference("Nose", "n", "Assets" + _basePartsSubUrl);
        _characterPartsReference.FacialList = UpdateCharacterReference("Facial", "fl", "Assets" + _basePartsSubUrl);


        EditorUtility.SetDirty(_characterPartsReference);
        AssetDatabase.SaveAssets();
    }





    #region Utility

    private static List<Texture2D> UpdateCharacterReference(string folderName, string filePattern, string charactersSubdirectory)
    {

        List<Texture2D> finalTextures = new List<Texture2D>();

        int imageCount = CountImagesInDirectory(folderName);
        for (int i = 0; i < imageCount; i++)
        {
            finalTextures.Add((Texture2D)AssetDatabase.LoadAssetAtPath(charactersSubdirectory + folderName + "/" + filePattern + i + ".png", typeof(Texture2D)));
        }
        return finalTextures;
    }



    private static int CountImagesInDirectory(string subDirectory)
    {
        UnityEngine.Debug.Log(Application.dataPath + subDirectory);
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath + _basePartsSubUrl + subDirectory);
        int count = dir.GetFiles().ToList().Where(file => file.Extension == ".png").Count();
        return count;
    }
    #endregion
}
