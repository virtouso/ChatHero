using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CommunicationGraphView : GraphView
{

    private NodeSearchWindow _nodeSearchWindow;

    public CommunicationGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        AddGrid();
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddSearchWindow();
    }

    private void AddSearchWindow()
    {
        _nodeSearchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _nodeSearchWindow.Init(this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _nodeSearchWindow);
    }

    private void AddGrid()
    {
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        styleSheets.Add(Resources.Load<StyleSheet>("CommunicationEditor"));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node) { compatiblePorts.Add(port); }
        });

        return compatiblePorts;
    }

    public PlayerNode GeneratePlayerNode(Vector2 mousePosition)
    {

        var node = new PlayerNode
        {
            title = "Player Dialogue",
            DialogueText = "dsdsa",
            GUID = Guid.NewGuid().ToString()
        };


        Toggle IsStarterToggle = new Toggle("Is Starter");
        IsStarterToggle.RegisterValueChangedCallback(val => { node.IsStartingDialogue = val.newValue; });
        node.contentContainer.Add(IsStarterToggle);

        Button AddChoiceButton = new Button(() => AddDialogueChoiceToNode(node));
        AddChoiceButton.text = "Add Choice";
        node.contentContainer.Add(AddChoiceButton);




        var inputPort = GeneratePlayerPort(node, Direction.Input);
        inputPort.portName = "Last Dialogue";
        node.inputContainer.Add(inputPort);



        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(mousePosition, CommunicationEditorReferences.CharacterDialogueNodeRectSize));
        return node;

    }

    public void AddDialogueChoiceToNode(PlayerNode playerNode)
    {
        var portResult = playerNode.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));// final param not mandatory. its for passing vari
        portResult.userData = Guid.NewGuid().ToString();
        Button removePortButton = new Button(() => { playerNode.outputContainer.Remove(portResult); });
        removePortButton.text = "X";
        portResult.contentContainer.Add(removePortButton);

        TextField dialogueField = new TextField();
        dialogueField.RegisterValueChangedCallback(val => { portResult.portName = val.newValue; });
        portResult.contentContainer.Add(dialogueField);

        playerNode.outputContainer.Add(portResult);
        playerNode.RefreshPorts();
        playerNode.RefreshExpandedState();

    }

    public CharacterNode GenerateCharacterNode(Vector2 mousePosition)
    {

        var node = new CharacterNode
        {
            title = "Character Dialogue",
            DialogueText = "Dialogue",
            GUID = Guid.NewGuid().ToString()
        };


        TextField characterNameField = new TextField();
        characterNameField.label = "CharacterName:";
        characterNameField.RegisterValueChangedCallback(val =>
        {
            node.CharacterName = val.newValue;
            node.title = val.newValue;
            node.titleContainer.userData = val.newValue;

        });
        node.titleContainer.Add(characterNameField);


        TextField userScoreField = new TextField("User Score", 1, false, false, char.MinValue);
        userScoreField.label = "Terminal User Score";
        userScoreField.value = "0";
        userScoreField.RegisterValueChangedCallback(val => { node.UserScore = int.Parse(GetNumbers(val.newValue)); });
        node.contentContainer.Add(userScoreField);

        Toggle IsStarterToggle = new Toggle("Is Starter");
        IsStarterToggle.RegisterValueChangedCallback(val => { node.IsStartingDialogue = val.newValue; });
        node.contentContainer.Add(IsStarterToggle);

        var inputPort = GenerateCharacterPort(node, Direction.Input);
        inputPort.portName = "Last Dialogue";
        node.inputContainer.Add(inputPort);


        var outputPort = GenerateCharacterPort(node, Direction.Output);
        outputPort.portName = "Next Dialogue";
        node.outputContainer.Add(outputPort);

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(mousePosition, CommunicationEditorReferences.CharacterDialogueNodeRectSize));
        return node;
    }

    private static string GetNumbers(string input)
    {
        return new string(input.Where(c => char.IsDigit(c)).ToArray());
    }

    private Port GeneratePlayerPort(PlayerNode playerNode, Direction portDirecction, Port.Capacity portCapacity = Port.Capacity.Single)
    {

        var portResult = playerNode.InstantiatePort(Orientation.Horizontal, portDirecction, portCapacity, typeof(float));// final param not mandatory. its for passing variable

        return portResult;

    }

    private Port GenerateCharacterPort(CharacterNode characterNode, Direction portDirection, Port.Capacity portCapacity = Port.Capacity.Single)
    {


        var resultPort = characterNode.InstantiatePort(Orientation.Horizontal, portDirection, portCapacity, typeof(float));// final param not mandatory. its for passing variable
        if (portDirection == Direction.Output)
        {

            TextField dialogueText = new TextField
            {
                name = string.Empty,
                value = "CharacterDialogue"
            };
            dialogueText.RegisterValueChangedCallback(val =>
            {
                resultPort.portName = val.newValue;

            });

            resultPort.contentContainer.Add(dialogueText);
        }
        return resultPort;
    }





}
