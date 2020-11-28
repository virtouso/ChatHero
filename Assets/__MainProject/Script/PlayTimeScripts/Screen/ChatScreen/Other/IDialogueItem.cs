using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueItem 
{
    void Init(Texture2D characterTexture, Rect textureRect, string dialogueText);
}
