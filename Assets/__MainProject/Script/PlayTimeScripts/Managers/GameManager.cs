using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IGameManager
{
    #region public
    public void AddtoScore(int score) 
    {
        _score += score;
        _scoreText.text = _score.ToString();
    }



    #endregion
    [SerializeField] private Text _scoreText;

    #region UnityRferences

    #endregion

    #region private
    private int _score;

    #endregion

   
}
