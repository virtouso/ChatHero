using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopUp
{
    void OnPopUpOpened(string title, string body, string buttonText,System.Action onOpenedAction,UnityEngine.Events.UnityAction closeButtonAction);
}
