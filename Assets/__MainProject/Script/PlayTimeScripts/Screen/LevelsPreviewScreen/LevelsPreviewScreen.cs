using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class LevelsPreviewScreen : Screen
{
    #region public
    public override void OnScreenOpened(System.Object basicData, Action<System.Object> onOpenedAction, UnityAction closeButtonAction)
    {
        print("levels review opened");
        getLevelsScriptableObject();

    }
    #endregion

    #region UnityReferences
    [SerializeField] private GameObject StartLevelButtonPrefab;
    [SerializeField] private Transform _startlevelButtonsParent;

    #endregion

    #region private
    IMenuManager _menuManager;
    StartLevelButton.Factory _startlevelButtonFactory;
    private string _levelsAddress = "Assets/__MainProject/Configuration/Communications/LevelsReferencesScriptableObject.asset";
    private string _levelButtonBaseText = "Level-";
    #endregion

    #region Unity Callbacks
    [Inject]
    public void Construct(IMenuManager menuManager, StartLevelButton.Factory startLevelButtonFactory)
    {
        _startlevelButtonFactory = startLevelButtonFactory;
        _menuManager = menuManager;
    }

    #endregion

    #region Utility

    private void getLevelsScriptableObject()
    {
        UtilityAddressables.LoadAssetByStringAddressAsync<LevelsReferences>(_levelsAddress, onGetLevelsScriptableObjectCompleted);
    }

    private void onGetLevelsScriptableObjectCompleted(AsyncOperationHandle<LevelsReferences> levelsResult)
    {

        List<CommunicarionContainer> levels = levelsResult.Result.Levels;
        for (int i = 0; i < levels.Count; i++)
        {
            StartLevelButton levelButton = _startlevelButtonFactory.Create();
            int levelIndex = i;
            levelButton.InitButton(string.Concat(_levelButtonBaseText, (i + 1)), delegate { openlevel(levels[levelIndex]); });
            levelButton.transform.SetParent(_startlevelButtonsParent, false);
        }
    }

    private void openlevel(CommunicarionContainer levelContainer)
    {

        gameObject.SetActive(false);
        _menuManager.ShowScreen(Screens.ChatScreen, levelContainer, null, null);
        Destroy(gameObject);
    }

    #endregion



    public class Factory : PlaceholderFactory<IMenuManager, StartLevelButton.Factory, LevelsPreviewScreen>
    {
        public Factory(IMenuManager x, StartLevelButton.Factory y)
        {

        }
    }



}
