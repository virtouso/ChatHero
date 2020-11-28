using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChoiceButton 
{

    void Init(string dialogue, UnityEngine.Events.UnityAction choiceSelectAction);

}
