using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CommunicationEditor : EditorWindow
{

    #region Variables
    private CommunicationGraphView _communicationGraphView;

    #endregion


    #region Unity Callbacks

    [MenuItem("ChatHero/ Communication Editor")]
    public static void OpenCommunicationEditor()
    {
        var window = GetWindow<CommunicationEditor>();
        window.titleContent = new GUIContent("Communication Editor");
    }



    private void OnEnable()
    {

    }


    private void OnDisable()
    {

    }

    #endregion






    #region Utility

    #endregion



}
