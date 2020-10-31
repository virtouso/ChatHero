using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fielName = "New Narrative";

    [MenuItem(EditorNamingReferences.DialogueEditorMenuBarDirection)]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.title = EditorNamingReferences.DialogueEditorTitle;

    }

    #region Unity Callbacks


    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    #endregion


    private void ConstructGraphView()
    {

        _graphView = new DialogueGraphView
        {
            name = EditorNamingReferences.DialogueGraphViewName
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }



    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });


        var nodeCreateButton = new Button(() => { _graphView.CreateNode(EditorNamingReferences.NewDialogueText); });
        nodeCreateButton.text = EditorNamingReferences.CreateDialogueNodeText;
        toolbar.Add(nodeCreateButton);
        rootVisualElement.Add(toolbar);

        var filenameTextField = new TextField("FileName::");
        filenameTextField.SetValueWithoutNotify("New Narrative");
        filenameTextField.MarkDirtyRepaint();
        filenameTextField.RegisterValueChangedCallback(val => _fielName = val.newValue);
        //   filenameTextField.StretchToParentSize();
        toolbar.Add(filenameTextField);








    }

    private void RequestDataOperation(bool save)
    {

        if (string.IsNullOrEmpty(_fielName))
        {
            EditorUtility.DisplayDialog("Invalid File Name!", "Please Enter a valid file name.", "OK");
            return; 
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            saveUtility.SaveGraph(_fielName);
        }
        else 
        {
            saveUtility.LoadGraph(_fielName);
        }
    }
}
 