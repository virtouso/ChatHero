using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CharacterEditorWindow : EditorWindow
{
    private Texture2D _finalTexture;
    private Texture2D backgroundImage;

    [MenuItem("ChatHero/ Character Editor")]
    public static void OpenCharacterEditorWindow()
    {
        var window = GetWindow<CharacterEditorWindow>();
        window.titleContent = new GUIContent("CharacterEditor");

    }



    #region Reference Variables
    private CharactersScriptableObject _charactersReference;
    private string _charactersScriptableObjectAddress = "Assets/__MainProject/Configuration/Characters/CharactersScriptableObject.asset";
    private const string _characterPartsScriptableObjectAddress = "Assets/__MainProject/Configuration/Character Parts/CharacterPartsScriptableObject.asset";
    private const string _basePartsSubUrl = "/__MainProject/Content/Image/UI/Character/";
    private static CharacterPartsScriptableObject _characterPartsReference;
    #endregion


    #region Variable Caches
    private static string _nameEnglish;
    private static string _namePersian;
    private static Color _skinColor;
    private static int _imageWidth = 128;
    private static int _imageHeight = 128;

    #endregion


    #region Index Caches
    private int _currentFaceIndex = 0;
    private int _currentClothingIndex = 0;
    private int _currentEyeIndex = 0;
    private int _currentEyebrowIndex = 0;
    private int _currentMouthIndex = 0;
    private int _currentEarIndex = 0;
    private int _currentEaringIndex = 0;
    private int _currentNoseIndex = 0;
    private int _currentFacialIndex = 0;
    #endregion





    private void OnEnable()
    {
        _charactersReference = (CharactersScriptableObject)AssetDatabase.LoadAssetAtPath(_charactersScriptableObjectAddress, typeof(CharactersScriptableObject));
        _characterPartsReference = (CharacterPartsScriptableObject)AssetDatabase.LoadAssetAtPath(_characterPartsScriptableObjectAddress, typeof(CharacterPartsScriptableObject));

    }
    private void OnDisable()
    {


    }



    Vector2 VerticalScrollPosition;
    private void OnGUI()
    {
        VerticalScrollPosition = EditorGUILayout.BeginScrollView(VerticalScrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));
        ShowFinalImage();
        ShowNames();
        ShowEdditingTexture("Face", _characterPartsReference.FaceList, _currentFaceIndex);
        ShowSkinColor();
        ShowEdditingTexture("Clothing", _characterPartsReference.ClothingList, _currentClothingIndex);
        ShowEdditingTexture("Eye", _characterPartsReference.EyeList, _currentEyeIndex);
        ShowEdditingTexture("Eyebrow", _characterPartsReference.EyebrowList, _currentEyebrowIndex);
        ShowEdditingTexture("Mouth", _characterPartsReference.MouthList, _currentMouthIndex);
        ShowEdditingTexture("Ear", _characterPartsReference.EarList, _currentEarIndex);
        ShowEdditingTexture("Earing", _characterPartsReference.EaringList, _currentEaringIndex);
        ShowEdditingTexture("Nose", _characterPartsReference.NoseList, _currentNoseIndex);
        ShowEdditingTexture("Facial", _characterPartsReference.FacialList, _currentFacialIndex);
        ShowModificationButtons();
        ShowCharactersList();


        EditorGUILayout.EndScrollView();
    }


    #region GuiLayOutReferences

    private void ShowFinalImage()
    {
        EditorGUILayout.BeginHorizontal();
        CharacterModel finalCharacter = new CharacterModel();
        finalCharacter.Face = _characterPartsReference.FaceList[_currentFaceIndex];
        finalCharacter.Clothing = _characterPartsReference.ClothingList[_currentClothingIndex];
        finalCharacter.Eye = _characterPartsReference.EyeList[_currentEyeIndex];
        finalCharacter.Eyebrow = _characterPartsReference.EyebrowList[_currentEyebrowIndex];
        finalCharacter.Mouth = _characterPartsReference.MouthList[_currentMouthIndex];
        finalCharacter.Ear = _characterPartsReference.EarList[_currentEarIndex];
        finalCharacter.Earing = _characterPartsReference.EaringList[_currentEaringIndex];
        finalCharacter.Nose = _characterPartsReference.NoseList[_currentNoseIndex];
        finalCharacter.Facial = _characterPartsReference.FacialList[_currentFacialIndex];


        EditorGUILayout.ObjectField("Final Result", MakeCharacter(finalCharacter, _imageWidth, _imageHeight, Color.white), typeof(Texture2D));
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }

    private void ShowNames()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name Persian");
        _namePersian = GUILayout.TextField(_namePersian);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name English");
        _nameEnglish = GUILayout.TextField(_nameEnglish);
        EditorGUILayout.EndHorizontal();
    }



    private static void ShowSkinColor()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Skin Color");
        _skinColor = EditorGUILayout.ColorField(_skinColor);
        EditorGUILayout.EndHorizontal();
    }

    private static void ShowEdditingTexture(string LabelName, List<Texture2D> editingTexture, int currentIndex)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(LabelName);



        EditorGUILayout.ObjectField("", editingTexture[currentIndex], typeof(Texture2D));

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("<")) { ScrollCharacterPart(true, editingTexture, currentIndex); }
        if (GUILayout.Button(">")) { ScrollCharacterPart(false, editingTexture, currentIndex); }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }


    private void ShowModificationButtons()
    {
        if (GUILayout.Button("Modify Current"))
        {
            ModifySelectedCharacter();
        }
        if (GUILayout.Button("Add New"))
        {
            AddNewCharacter();
        }

    }

    private void ShowCharactersList()
    {
        EditorGUILayout.LabelField("Characters List");

        for (int i = 0; i < _charactersReference.Characters.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField((i + 1).ToString());

            EditorGUILayout.LabelField(_charactersReference.Characters[i].NameEnglish);
            EditorGUILayout.LabelField(_charactersReference.Characters[i].NamePersian);
            EditorGUILayout.ObjectField("", MakeCharacter(_charactersReference.Characters[i], _imageWidth, _imageHeight, Color.white), typeof(Texture2D));
            EditorGUILayout.EndHorizontal();
        }
    }
    #endregion
    #region Utility


    private void ModifySelectedCharacter()
    {
        if (!_charactersReference.Characters.Any(x => x.NameEnglish == _nameEnglish))
        {

            EditorUtility.DisplayDialog("error", "there is no character with this name", "OK");
            return;

        }

        CharacterModel selectedCharacter = _charactersReference.Characters.First(x => x.NameEnglish == _nameEnglish);
        selectedCharacter.NamePersian = _namePersian;
        selectedCharacter.Face = _characterPartsReference.FaceList[_currentFaceIndex];
        selectedCharacter.FaceColor = _skinColor;
        selectedCharacter.Clothing = _characterPartsReference.ClothingList[_currentClothingIndex];
        selectedCharacter.Eye = _characterPartsReference.EyeList[_currentEyeIndex];
        selectedCharacter.Eyebrow = _characterPartsReference.EyebrowList[_currentEyebrowIndex];
        selectedCharacter.Mouth = _characterPartsReference.MouthList[_currentMouthIndex];
        selectedCharacter.Ear = _characterPartsReference.EarList[_currentEarIndex];
        selectedCharacter.Earing = _characterPartsReference.EaringList[_currentEaringIndex];
        selectedCharacter.Nose = _characterPartsReference.NoseList[_currentNoseIndex];
        selectedCharacter.Facial = _characterPartsReference.FacialList[_currentFacialIndex];

        EditorUtility.SetDirty(_charactersReference);
        AssetDatabase.SaveAssets();

    }

    private void AddNewCharacter()
    {
        if (_charactersReference.Characters.Any(x => x.NameEnglish == _nameEnglish) || _charactersReference.Characters.Any(x => x.NamePersian == _namePersian))
        {

            EditorUtility.DisplayDialog("error", "Character Name Already Exist", "OK");
            return;
        }

        CharacterModel newCharacter = new CharacterModel();

        newCharacter.NamePersian = _namePersian;
        newCharacter.NameEnglish = _nameEnglish;
        newCharacter.Face = _characterPartsReference.FaceList[_currentFaceIndex];
        newCharacter.FaceColor = _skinColor;
        newCharacter.Clothing = _characterPartsReference.ClothingList[_currentClothingIndex];
        newCharacter.Eye = _characterPartsReference.EyeList[_currentEyeIndex];
        newCharacter.Eyebrow = _characterPartsReference.EyebrowList[_currentEyebrowIndex];
        newCharacter.Mouth = _characterPartsReference.MouthList[_currentMouthIndex];
        newCharacter.Ear = _characterPartsReference.EarList[_currentEarIndex];
        newCharacter.Earing = _characterPartsReference.EaringList[_currentEaringIndex];
        newCharacter.Nose = _characterPartsReference.NoseList[_currentNoseIndex];
        newCharacter.Facial = _characterPartsReference.FacialList[_currentFacialIndex];

        _charactersReference.Characters.Add(newCharacter);

        EditorUtility.SetDirty(_charactersReference);
        AssetDatabase.SaveAssets();
    }

    private static void ScrollCharacterPart(bool additiveScroll, List<Texture2D> scrollingList, int currentIndex)
    {
        if (additiveScroll)
        {

            if (currentIndex < scrollingList.Count - 2) { currentIndex++; }
            else { currentIndex = 0; }

        }
        else
        {
            if (currentIndex > 0) { currentIndex--; }
            else { currentIndex = scrollingList.Count - 1; }
        }


    }

    private static Texture2D MakeCharacter(CharacterModel inputCharacter, int goalWidth, int goalHeight, Color backgroundColor)
    {
        Texture2D finalTexture = new Texture2D(goalWidth, goalHeight);


        for (int i = 0; i < goalWidth; i++)
        {
            for (int j = 0; j < goalHeight; j++)
            {

                finalTexture.SetPixel(i, j, backgroundColor);

                Color pixelColor = inputCharacter.Face.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor + _skinColor);

                pixelColor = inputCharacter.Clothing.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

                pixelColor = inputCharacter.Ear.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor + pixelColor);

                pixelColor = inputCharacter.Earing.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

                pixelColor = inputCharacter.Nose.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor + pixelColor);

                pixelColor = inputCharacter.Mouth.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

                pixelColor = inputCharacter.Eye.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

                pixelColor = inputCharacter.Eyebrow.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);


                pixelColor = inputCharacter.Facial.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

            }
        }


        return finalTexture;
    }



    #endregion


}
