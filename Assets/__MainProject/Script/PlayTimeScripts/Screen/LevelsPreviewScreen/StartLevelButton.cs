using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour,IStartLevelButton
{
    [SerializeField] private Text _buttonText;
    [SerializeField] private Button _buttonReference;


   public void InitButton(string buttonText, UnityEngine.Events.UnityAction buttonPressedAction) 
    {
        _buttonText.text = buttonText;
        _buttonReference.onClick.AddListener(buttonPressedAction);
    }



    public class Factory : Zenject.PlaceholderFactory<StartLevelButton>
    {

    }

}
