using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class ChatScreen : Screen
{
    #region public
    public override void OnScreenOpened(System.Object basicData, Action<System.Object> onOpenedAction, UnityAction closeButtonAction)
    {
        print("chat screen opened called");

        //todo init characters

        _levelData = (CommunicarionContainer)basicData;
        InitCharacters();

    }
    #endregion


    #region Unity References
    [SerializeField] private Transform _chatItemsParent;
    [SerializeField] private Transform _choicesParent;
    [SerializeField] private Sprite NoImageSprite;
    #endregion

    #region private
    private IMenuManager _menuManager;
    private CommunicarionContainer _levelData;
    private string _charactersListAddress = "Assets/__MainProject/Configuration/Characters/CharactersScriptableObject.asset";
    private CharactersScriptableObject _charactersList;
    private ChoiceButton.Factory _choiceButtonFactory;
    private DialogueItem.Factory _playerDialogueItemFactory;
    private DialogueItem.Factory _characterDialogueItemFactory;
    #endregion



    #region unity callbacks
    [Inject]
    public void Construct([Inject] IMenuManager menuManager, [Inject(Id = "Player")] DialogueItem.Factory playerDialogueItemFactory, [Inject(Id = "Character")] DialogueItem.Factory characterDialogueItemFactory, [Inject] ChoiceButton.Factory choiceButtonFactory)
    {
        _menuManager = menuManager;
        _choiceButtonFactory = choiceButtonFactory;
        _playerDialogueItemFactory = playerDialogueItemFactory;
        _characterDialogueItemFactory = characterDialogueItemFactory;
    }

    #endregion

    #region Utility

    private void InitCharacters()
    {
        UtilityAddressables.LoadAssetByStringAddressAsync<CharactersScriptableObject>(_charactersListAddress, onCharactersAddressResolved);
    }

    private void onCharactersAddressResolved(AsyncOperationHandle<CharactersScriptableObject> resolvedObject)
    {
        _charactersList = resolvedObject.Result;
        InitCommunication();

    }

    private void InitCommunication()
    {
        CharacterDialogue firstDialogue = _levelData.CharacterDialogues.First(x => x.IsStartingDialogue);
        PlayCharacterCommuniction(firstDialogue);
    }

    private void PlayCharacterCommuniction(CharacterDialogue characterNode)
    {
        DialogueItem dialogueItemResult = _characterDialogueItemFactory.Create();
        dialogueItemResult.transform.SetParent(_chatItemsParent, false);

        string characterName = characterNode.CharacterName;
        CharacterModel character = _charactersList.Characters.First(x => x.Name.ToLower() == characterName.ToLower());
        Texture2D characterTexture = UtilityCharacter.MakeCharacter(character, character.FaceColor, BaseCharacterData.ImageWidth, BaseCharacterData.ImageHeight, Color.blue);
        dialogueItemResult.Init(characterTexture, new Rect(0, 0, BaseCharacterData.ImageWidth, BaseCharacterData.ImageHeight), characterNode.Dialogue);

        if (characterNode.UserScore > 0)
        {
            //todo give the score finish the game
            print("we will give you the score S");
            _menuManager.UpdateScore(ScoreData.ChatLevelWinScore);
          StartCoroutine(  returnToLevelsMenu());
            return;
        }

        EdgeModel characterEdge = _levelData.Edges.First(x => x.BaseNodeGuid == characterNode.GUID);
        MakePlayerChoices(characterEdge.TargetNodeGuid);


    }



    private void MakePlayerChoices(string playerNodeGuid)
    {
        List<EdgeModel> playerNodeDialogueEdges = _levelData.Edges.Where(x => x.BaseNodeGuid == playerNodeGuid).ToList();

        foreach (var item in playerNodeDialogueEdges)
        {
            ChoiceButton choiceButton = _choiceButtonFactory.Create();
            choiceButton.Init(item.PortName, delegate { choiceButtonPressed(item); });
            choiceButton.transform.SetParent(_choicesParent, false);
        }
    }



    private void choiceButtonPressed(EdgeModel playerNodeEdge)
    {
        ClearChoices();
        PlayPlayerCommunication(playerNodeEdge);
        CharacterDialogue characterNode = _levelData.CharacterDialogues.First(x => x.GUID == playerNodeEdge.TargetNodeGuid);
        PlayCharacterCommuniction(characterNode);

    }



    private void PlayPlayerCommunication(EdgeModel playerNodeEdge)
    {
        DialogueItem dialogueItemResult = _playerDialogueItemFactory.Create();
        dialogueItemResult.transform.SetParent(_chatItemsParent, false);
        dialogueItemResult.Init(NoImageSprite.texture, new Rect(0, 0, BaseCharacterData.ImageWidth, BaseCharacterData.ImageHeight), playerNodeEdge.PortName);

    }

    private void ClearChoices()
    {

        for (int i = 0; i < _choicesParent.childCount; i++)
        {
            Destroy(_choicesParent.GetChild(i).gameObject);
        }
    }

    private IEnumerator returnToLevelsMenu()
    {
        yield return new WaitForSeconds(2);
        _menuManager.ShowScreen(Screens.LevelsPreviwScreen, null, null,null) ;
        Destroy(gameObject);
    }

    #endregion



    public class Factory : PlaceholderFactory<IMenuManager, DialogueItem.Factory, DialogueItem.Factory, ChoiceButton.Factory, ChatScreen>
    {
        public Factory([Inject] IMenuManager menuManager, [Inject(Id = "Player")] DialogueItem.Factory playerDialogueItemFactory, [Inject(Id = "Character")] DialogueItem.Factory characterDialogueItemFactory, ChoiceButton.Factory choiceButtonFactory)
        {

        }
    }
}
