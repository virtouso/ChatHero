using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityCharacter
{
    public static Texture2D MakeCharacter(CharacterModel inputCharacter, Color skinColor, int goalWidth, int goalHeight, Color backgroundColor)
    {
        Texture2D finalTexture = new Texture2D(goalWidth, goalHeight);


        for (int i = 0; i < goalWidth; i++)
        {
            for (int j = 0; j < goalHeight; j++)
            {

                finalTexture.SetPixel(i, j, backgroundColor);

                Color pixelColor = inputCharacter.Face.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor * skinColor);

                pixelColor = inputCharacter.Clothing.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

                pixelColor = inputCharacter.Nose.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor * skinColor);

                pixelColor = inputCharacter.Mouth.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

                pixelColor = inputCharacter.Eye.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

                pixelColor = inputCharacter.Eyebrow.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);


                pixelColor = inputCharacter.Facial.GetPixel(i, j);
                if (pixelColor.a > 0)
                    finalTexture.SetPixel(i, j, pixelColor);

            }
        }
        finalTexture.Apply();


        return finalTexture;
    }

}
