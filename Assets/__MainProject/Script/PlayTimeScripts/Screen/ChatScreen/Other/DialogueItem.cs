using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DialogueItem : MonoBehaviour, IDialogueItem
{

    [SerializeField] private Text _dialogueText;
    [SerializeField] private Image _characterSprite;


    public void Init(Texture2D characterTexture, Rect textureRect, string dialogueText)
    {
        _dialogueText.text = dialogueText;
        _characterSprite.sprite = Sprite.Create(characterTexture, textureRect, Vector2.zero);
    }



    public class Factory : PlaceholderFactory<DialogueItem>
    {
        public Factory()
        {

        }

    }

}
