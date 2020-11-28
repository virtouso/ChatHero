using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Zenject;

public class PopUp : MonoBehaviour,IPopUp
{
    #region Public

    public void OnPopUpOpened(string title, string body, string buttonText, System.Action onOpenedAction, UnityEngine.Events.UnityAction closeButtonAction)
    {
        _titleText.text = title;
        _bodyText.text = body;
        _closeButtonText.text = buttonText;
        _onOpened += onOpenedAction;

        if ( _onOpened != null ){ _onOpened.Invoke(); }
        _onClosePressed += closeThisPopUp;
        _onClosePressed += closeButtonAction;
        _closeButton.onClick.AddListener(_onClosePressed);
    }

    #endregion

    #region Unity References
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _bodyText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Text _closeButtonText;
    #endregion

    #region private
    private Action _onOpened;
    private UnityAction _onClosePressed;
    #endregion

    #region Unity Callbacks

    #endregion




    #region utility
    private void closeThisPopUp()
    {
        gameObject.SetActive(false);

    }
    #endregion
   



    public class Factory : PlaceholderFactory<PopUp>
    {

    }
}
