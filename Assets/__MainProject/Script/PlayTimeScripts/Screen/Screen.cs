using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Screen :MonoBehaviour,  IScreen
{
 

    public virtual void OnScreenOpened(System.Object basicData, Action<System.Object> onOpenedAction, UnityAction closeButtonAction)
    {
    }

 
}
