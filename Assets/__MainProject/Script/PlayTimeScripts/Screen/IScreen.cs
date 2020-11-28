using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IScreen 
{
    void OnScreenOpened(System.Object basicData, Action<System.Object> onOpenedAction, UnityEngine.Events.UnityAction closeButtonAction);

}
