using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CommunicationEditor : EditorWindow
{

    #region Variables
    public static CommunicationEditor CommunicationEditorWindow;
    private CommunicationGraphView _communicationGraphView;
    private string saveLoadTextValue;
    #endregion


    #region Unity Callbacks

    [MenuItem("ChatHero/ Communication Editor")]
    public static void OpenCommunicationEditor()
    {
        CommunicationEditorWindow = GetWindow<CommunicationEditor>();
        CommunicationEditorWindow.titleContent = new GUIContent("Communication Editor");
    }



    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {

    }

    #endregion






    #region Utility

    private void GenerateToolbar()
    {

        var toolBar = new Toolbar();

        var saveButton = new Button(() => SaveLoadOperation(true, saveLoadTextValue));
        saveButton.text = "SAVE";
        toolBar.Add(saveButton);

        var loadButton = new Button(() => SaveLoadOperation(false, saveLoadTextValue));
        loadButton.text = "LOAD";
        toolBar.Add(loadButton);

        var saveLoadTextField = new TextField();
        saveLoadTextField.SetValueWithoutNotify("New Communication");
        saveLoadTextField.MarkDirtyRepaint();
        saveLoadTextField.RegisterValueChangedCallback(val => { saveLoadTextValue = val.newValue; });
        toolBar.Add(saveLoadTextField);

        rootVisualElement.Add(toolBar);
    }

    private void SaveLoadOperation(bool save, string saveLoadTextValue)
    {
        if (string.IsNullOrEmpty(saveLoadTextValue))
        {
            EditorUtility.DisplayDialog("invalid file name", "please enter a valid file name", "OK!!!");
            return;
        }
        var saveUtility = DataOperationUtility.GetInstance(_communicationGraphView);
        if (save) { saveUtility.SaveGraph(saveLoadTextValue); }
        else { saveUtility.LoadGraph(saveLoadTextValue); }
    }

    private void ConstructGraphView()
    {
        _communicationGraphView = new CommunicationGraphView
        {
            name = "Communication Graph Editor"
        };
        _communicationGraphView.StretchToParentSize();
        rootVisualElement.Add(_communicationGraphView);
    }




    #endregion



}
