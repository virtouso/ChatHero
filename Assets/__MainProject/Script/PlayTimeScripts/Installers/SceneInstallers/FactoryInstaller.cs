using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class FactoryInstaller : MonoInstaller<FactoryInstaller>
{
    [SerializeField] private GameObject _popUp;
    [SerializeField] private Screen _levelsPreviewScreen;
    [SerializeField] private Screen _chatScreen;
    [SerializeField] private GameObject _startLevelButtonPrefab;
    [SerializeField] private GameObject _chatChoicePrefab;
    [SerializeField] private GameObject _chatPlayerItemPrefab;
    [SerializeField] private GameObject _chatCharacterItemPrefab;
    Dictionary<Screens, Screen> ScreensDictionary;
    public override void InstallBindings()
    {
       

        Container.Bind<IPopUp>().To<PopUp>().AsSingle();


        GameObject popInstance = Instantiate(_popUp);
        Container.BindFactory<PopUp, PopUp.Factory>().FromComponentOn(popInstance);

        Container.BindFactory<IMenuManager, StartLevelButton.Factory, LevelsPreviewScreen, LevelsPreviewScreen.Factory>().FromComponentInNewPrefab(_levelsPreviewScreen.gameObject);
        Container.BindFactory<IMenuManager, DialogueItem.Factory, DialogueItem.Factory, ChoiceButton.Factory, ChatScreen, ChatScreen.Factory>().FromComponentInNewPrefab(_chatScreen.gameObject);
        Container.BindFactory<StartLevelButton, StartLevelButton.Factory>().FromComponentInNewPrefab(_startLevelButtonPrefab);
        Container.BindFactory<ChoiceButton, ChoiceButton.Factory>().FromComponentInNewPrefab(_chatChoicePrefab);
        Container.BindFactory<DialogueItem, DialogueItem.Factory>().WithId("Player").FromComponentInNewPrefab(_chatPlayerItemPrefab);
        Container.BindFactory<DialogueItem, DialogueItem.Factory>().WithId("Character").FromComponentInNewPrefab(_chatCharacterItemPrefab);





    }





}