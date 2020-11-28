using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestClass : MonoBehaviour
{

    // [Inject] private readonly SignalBus _signalBus;

    private IMenuManager _menuManager;


    [Inject]
    public void Construct([Inject]IMenuManager menuManager) { _menuManager = menuManager; }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //_menuManager.ShowPopUp("","","",null,null);
            // _signalBus.Fire(new ScoreUpdated(10));
            print("ddddd");
            // _menuManager.ShowPopUp("moeen", "hay you", "jesi", null, null);
            _menuManager.ShowScreen(Screens.LevelsPreviwScreen,null,null,null);
        }
    }
}
