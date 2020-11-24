using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{

    private CommunicationGraphView _communicationGraphView;



    public void Init( CommunicationGraphView communicationGraphView)
    {
        _communicationGraphView = communicationGraphView;
     
    }


    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Node Element"),0),
            new SearchTreeEntry(new GUIContent("Add Player Node")){userData=1,level=1},
            new SearchTreeEntry(new GUIContent("Add Character Node")){userData=2,level=1 }
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {

        var worldMousePosition = CommunicationEditor.CommunicationEditorWindow.rootVisualElement.ChangeCoordinatesTo(CommunicationEditor.CommunicationEditorWindow.rootVisualElement.parent, context.screenMousePosition - CommunicationEditor.CommunicationEditorWindow.position.position);
        var localMousePosition = _communicationGraphView.contentViewContainer.WorldToLocal(worldMousePosition);


        switch (SearchTreeEntry.userData)
        {
            case 1:
                UnityEngine.Debug.Log("Add Player Node Pressed");
            var playerResult=    _communicationGraphView.GeneratePlayerNode(localMousePosition);
                _communicationGraphView.AddElement(playerResult);

                return true;
            case 2:
                UnityEngine.Debug.Log("Add Character Node Pressed");
                var characterResult = _communicationGraphView.GenerateCharacterNode(localMousePosition);
                _communicationGraphView.AddElement(characterResult);
                return true;
            default:
                return false;
        }
    }
}
