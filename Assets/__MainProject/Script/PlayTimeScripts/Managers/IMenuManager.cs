using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IMenuManager
{
    void ShowPopUp(string title, string body, string buttonText,System.Action onOpenedAction,  UnityEngine.Events.UnityAction buttonAction);
    void ShowScreen(Screens screenName, System.Object basicData, Action<System.Object> onOpened, UnityEngine.Events.UnityAction onCloseAction);
    void UpdateScore(int score);
}
