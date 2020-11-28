using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class ChoiceButton : MonoBehaviour, IChoiceButton
{
    [SerializeField] private Text _choiceDialogueText;
    [SerializeField] private Button _choiceSelectButton;

    public void Init(string dialogue, UnityAction choiceSelectAction)
    {
        _choiceDialogueText.text = dialogue;
        _choiceSelectButton.onClick.AddListener(choiceSelectAction);

    }



    public class Factory: PlaceholderFactory<ChoiceButton> 
    {
        public Factory()
        {

        }
    }

}
