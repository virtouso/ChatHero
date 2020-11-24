using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DataOperationUtility
{
    private CommunicationGraphView _targetGraphView;
    private static string _baseCommunicationsDirectory = "Assets/__MainProject/Configuration/Communications/";
    private CommunicarionContainer _communicationContainer;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<PlayerNode> PlayerNodes => _targetGraphView.nodes.ToList().Where(x => x.GetType() == typeof(PlayerNode)).Cast<PlayerNode>().ToList();
    private List<CharacterNode> CharacterNodes => _targetGraphView.nodes.ToList().Where(x => x.GetType() == typeof(CharacterNode)).Cast<CharacterNode>().ToList();


    public static DataOperationUtility GetInstance(CommunicationGraphView targetGraphView)
    {
        return new DataOperationUtility
        {
            _targetGraphView = targetGraphView
        };
    }


    public void SaveGraph(string fileName)
    {


        if (!Edges.Any()) { return; }

        var communicationContainer = ScriptableObject.CreateInstance<CommunicarionContainer>();
        var connectedEdges = Edges.Where(x => x.input.node != null).ToArray();

        for (int i = 0; i < connectedEdges.Length; i++)
        {

            var outputNode = (connectedEdges[i].output.node as DialogueNode);
            var inputNode = (connectedEdges[i].input.node as DialogueNode);


            communicationContainer.Edges.Add(new EdgeModel
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedEdges[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });

        }


        communicationContainer.PlayerDialogues = new List<PlayerDialogue>();
        communicationContainer.CharacterDialogues = new List<CharacterDialogue>();
        foreach (var item in CharacterNodes)
        {
            communicationContainer.CharacterDialogues.Add(new CharacterDialogue
            {
                GUID = item.GUID,
                PositionInNodeEditor = item.GetPosition().position,
                IsStartingDialogue = item.IsStartingDialogue,
                CharacterName= item.CharacterName,
                UserScore= item.UserScore

            });
        }
        foreach (var item in PlayerNodes)
        {
            communicationContainer.PlayerDialogues.Add(new PlayerDialogue
            {
                GUID = item.GUID,
                IsStartingDialogue = item.IsStartingDialogue,
                PositionInNodeEditor = item.GetPosition().position
            });
        }
        AssetDatabase.CreateAsset(communicationContainer, _baseCommunicationsDirectory + $"{fileName}.asset");
        AssetDatabase.SaveAssets();
    }
    public void LoadGraph(string fileName)
    {

        EditorUtility.DisplayDialog("Not Made", "Still Not Implemented", "OK!!!");
        return;
        _communicationContainer = UnityEngine.AddressableAssets.Addressables.LoadAsset<CommunicarionContainer>(_baseCommunicationsDirectory + $"{fileName}.asset").Result;
        ClearGraph();
        GenerateDialogueNodes();
        ConnectDialogueNodes();
    }

    private void ConnectDialogueNodes()
    {
        throw new NotImplementedException();
    }

    private void ClearGraph()
    {

        foreach (var perNode in _targetGraphView.nodes.ToList())
        {
            Edges.Where(x => x.input.node == perNode).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(perNode);
        }
    }



    private void GenerateDialogueNodes()
    {

    }



}
