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
    private static string _name;
    // private static string _namePersian;
    private static Color _skinColor;
    private static int _imageWidth = 256;
    private static int _imageHeight = 256;
    private static Texture2D _finalCharacterImageCache;
    #endregion


    #region Index Caches
    private static int _currentFaceIndex = 0;
    private static int _currentClothingIndex = 0;
    private static int _currentEyeIndex = 0;
    private static int _currentEyebrowIndex = 0;
    private static int _currentMouthIndex = 0;
    private static int _currentNoseIndex = 0;
    private static int _currentFacialIndex = 0;
    #endregion





    private void OnEnable()
    {
        _charactersReference = (CharactersScriptableObject)AssetDatabase.LoadAssetAtPath(_charactersScriptableObjectAddress, typeof(CharactersScriptableObject));
        _characterPartsReference = (CharacterPartsScriptableObject)AssetDatabase.LoadAssetAtPath(_characterPartsScriptableObjectAddress, typeof(CharacterPartsScriptableObject));
        _finalCharacterImageCache = MakeCharacter(MakeFinalCharacterModel(), _imageWidth, _imageHeight, Color.blue);
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
        ShowEdditingTexture("Face", _characterPartsReference.FaceList, _currentFaceIndex, out _currentFaceIndex);
        ShowSkinColor();
        ShowEdditingTexture("Clothing", _characterPartsReference.ClothingList, _currentClothingIndex, out _currentClothingIndex);
        ShowEdditingTexture("Eye", _characterPartsReference.EyeList, _currentEyeIndex, out _currentEyeIndex);
        ShowEdditingTexture("Eyebrow", _characterPartsReference.EyebrowList, _currentEyebrowIndex, out _currentEyebrowIndex);
        ShowEdditingTexture("Mouth", _characterPartsReference.MouthList, _currentMouthIndex, out _currentMouthIndex);
        ShowEdditingTexture("Nose", _characterPartsReference.NoseList, _currentNoseIndex, out _currentNoseIndex);
        ShowEdditingTexture("Facial", _characterPartsReference.FacialList, _currentFacialIndex, out _currentFacialIndex);
        ShowModificationButtons();
        ShowCharactersList();


        EditorGUILayout.EndScrollView();
    }


    #region GuiLayOutReferences

    private void ShowFinalImage()
    {
        EditorGUILayout.BeginHorizontal();
        CharacterModel finalCharacter = MakeFinalCharacterModel();
        EditorGUILayout.ObjectField("Final Result", _finalCharacterImageCache, typeof(Texture2D));
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }

    private static CharacterModel MakeFinalCharacterModel()
    {
        CharacterModel finalCharacter = new CharacterModel();
        finalCharacter.Face = _characterPartsReference.FaceList[_currentFaceIndex];
        finalCharacter.Clothing = _characterPartsReference.ClothingList[_currentClothingIndex];
        finalCharacter.Eye = _characterPartsReference.EyeList[_currentEyeIndex];
        finalCharacter.Eyebrow = _characterPartsReference.EyebrowList[_currentEyebrowIndex];
        finalCharacter.Mouth = _characterPartsReference.MouthList[_currentMouthIndex];
        finalCharacter.Nose = _characterPartsReference.NoseList[_currentNoseIndex];
        finalCharacter.Facial = _characterPartsReference.FacialList[_currentFacialIndex];
        return finalCharacter;
    }

    private void ShowNames()
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name");
        _name = GUILayout.TextField(_name);
        EditorGUILayout.EndHorizontal();
    }



    private static void ShowSkinColor()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Skin Color");
        _skinColor = EditorGUILayout.ColorField(_skinColor);
        EditorGUILayout.EndHorizontal();
    }

    private static void ShowEdditingTexture(string LabelName, List<Texture2D> editingTexture, int currentIndex, out int resultIndex)
    {
        resultIndex = currentIndex;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(LabelName);
        EditorGUILayout.ObjectField("", editingTexture[currentIndex], typeof(Texture2D));

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("<"))
        {
            resultIndex = ScrollCharacterPart(true, editingTexture, currentIndex);
            _finalCharacterImageCache = MakeCharacter(MakeFinalCharacterModel(), _imageWidth, _imageHeight, Color.blue);

        }
        if (GUILayout.Button(">"))
        {
            resultIndex = ScrollCharacterPart(false, editingTexture, currentIndex);
            _finalCharacterImageCache = MakeCharacter(MakeFinalCharacterModel(), _imageWidth, _imageHeight, Color.blue);

        }
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

            EditorGUILayout.LabelField(_charactersReference.Characters[i].Name);
            //  EditorGUILayout.ObjectField("", MakeCharacter(_charactersReference.Characters[i], _imageWidth, _imageHeight, Color.white), typeof(Texture2D));
            EditorGUILayout.EndHorizontal();
        }
    }
    #endregion
    #region Utility


    private void ModifySelectedCharacter()
    {
        if (!_charactersReference.Characters.Any(x => x.Name == _name))
        {

            EditorUtility.DisplayDialog("error", "there is no character with this name", "OK");
            return;

        }

        CharacterModel selectedCharacter = _charactersReference.Characters.First(x => x.Name == _name);
        selectedCharacter.Face = _characterPartsReference.FaceList[_currentFaceIndex];
        selectedCharacter.FaceColor = _skinColor;
        selectedCharacter.Clothing = _characterPartsReference.ClothingList[_currentClothingIndex];
        selectedCharacter.Eye = _characterPartsReference.EyeList[_currentEyeIndex];
        selectedCharacter.Eyebrow = _characterPartsReference.EyebrowList[_currentEyebrowIndex];
        selectedCharacter.Mouth = _characterPartsReference.MouthList[_currentMouthIndex];
        //   selectedCharacter.Ear = _characterPartsReference.EarList[_currentEarIndex];
        //  selectedCharacter.Earing = _characterPartsReference.EaringList[_currentEaringIndex];
        selectedCharacter.Nose = _characterPartsReference.NoseList[_currentNoseIndex];
        selectedCharacter.Facial = _characterPartsReference.FacialList[_currentFacialIndex];

        EditorUtility.SetDirty(_charactersReference);
        AssetDatabase.SaveAssets();

    }

    private void AddNewCharacter()
    {
        if (_charactersReference.Characters.Any(x => x.Name == _name))
        {

            EditorUtility.DisplayDialog("error", "Character Name Already Exist", "OK");
            return;
        }

        CharacterModel newCharacter = new CharacterModel();

        newCharacter.Name = _name;
        newCharacter.Face = _characterPartsReference.FaceList[_currentFaceIndex];
        newCharacter.FaceColor = _skinColor;
        newCharacter.Clothing = _characterPartsReference.ClothingList[_currentClothingIndex];
        newCharacter.Eye = _characterPartsReference.EyeList[_currentEyeIndex];
        newCharacter.Eyebrow = _characterPartsReference.EyebrowList[_currentEyebrowIndex];
        newCharacter.Mouth = _characterPartsReference.MouthList[_currentMouthIndex];
        // newCharacter.Ear = _characterPartsReference.EarList[_currentEarIndex];
        // newCharacter.Earing = _characterPartsReference.EaringList[_currentEaringIndex];
        newCharacter.Nose = _characterPartsReference.NoseList[_currentNoseIndex];
        newCharacter.Facial = _characterPartsReference.FacialList[_currentFacialIndex];

        _charactersReference.Characters.Add(newCharacter);

        EditorUtility.SetDirty(_charactersReference);
        AssetDatabase.SaveAssets();
    }

    private static int ScrollCharacterPart(bool additiveScroll, List<Texture2D> scrollingList, int currentIndex)
    {
        UnityEngine.Debug.Log("Button Pressed");

        if (additiveScroll)
        {

            if (currentIndex < scrollingList.Count - 1) { currentIndex++; }
            else { currentIndex = 0; }

        }
        else
        {
            if (currentIndex > 0) { currentIndex--; }
            else { currentIndex = scrollingList.Count - 1; }
        }

        return currentIndex;
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
                    finalTexture.SetPixel(i, j, pixelColor * _skinColor);

                pixelColor = inputCharacter.Clothing.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);



                pixelColor = inputCharacter.Nose.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor * _skinColor);

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
                //  finalTexture.SetPixel(i, j, Color.red);
            }
        }
        finalTexture.Apply();


        return finalTexture;
    }



    #endregion


}
