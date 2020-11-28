using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class MenuManager : MonoBehaviour, IMenuManager
{

    #region Public

    public void ShowPopUp(string title, string body, string buttonText, Action onOpenedAction, UnityEngine.Events.UnityAction closeButtonAction)
    {
        PopUp popup = _popupFactory.Create();
        popup.transform.SetParent(_popupParent, false);
        popup.OnPopUpOpened(title, body, buttonText, onOpenedAction, closeButtonAction);

    }

    public void ShowScreen(Screens screen, System.Object basicData, Action<System.Object> onOpened, UnityEngine.Events.UnityAction onClose)
    {
      
        Screen oppeningScreen = ScreensDictionary[screen].Invoke();
        oppeningScreen.gameObject.SetActive(true);
        oppeningScreen.transform.SetParent(_screenParent, false);
        oppeningScreen.OnScreenOpened(basicData, onOpened, onClose);

    }

    public void UpdateScore(int score)
    {
        _signalBus.Fire(new ScoreUpdated(score));
    }

    #endregion

    #region Unity References
    [SerializeField] private Transform _popupParent;
    [SerializeField] private Transform _screenParent;
    #endregion

    #region Private

    [Inject] private SignalBus _signalBus;
    [Inject] private PopUp.Factory _popupFactory;
    [Inject] private StartLevelButton.Factory _startLevelButtonFactory;
    [Inject(Id = "Player")] DialogueItem.Factory _playerDialogueItemFactory;
    [Inject(Id = "Character")] DialogueItem.Factory _characterDialogueItemFactory;
    [Inject] ChoiceButton.Factory _choiceButtonFactory;
    [Inject] private LevelsPreviewScreen.Factory _levelsPreviewScreenFactory;
    [Inject] private ChatScreen.Factory _chatScreenFactory;
    private Dictionary<Screens, GenerateScreen> ScreensDictionary;

    #endregion


    #region Unity callbacks
    private void Start()
    {
        InitScreenDictionary();
        ShowScreen(Screens.LevelsPreviwScreen,null,null,null);
    }

    #endregion

    #region Utility
    private delegate Screen GenerateScreen();
    private Screen ShowLevelsPreviewScreen()
    {

        return _levelsPreviewScreenFactory.Create(this, _startLevelButtonFactory);
    }
    private Screen ShowChatScreen()
    {
        return _chatScreenFactory.Create(this, _playerDialogueItemFactory, _characterDialogueItemFactory, _choiceButtonFactory);
    }

    private void InitScreenDictionary()
    {
        ScreensDictionary = new Dictionary<Screens, GenerateScreen>();
        ScreensDictionary.Add(Screens.LevelsPreviwScreen, () => ShowLevelsPreviewScreen()); 
        ScreensDictionary.Add(Screens.ChatScreen, () => ShowChatScreen());

    }
    #endregion


}
