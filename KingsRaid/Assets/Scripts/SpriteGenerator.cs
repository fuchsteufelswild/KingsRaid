using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
    private static SpriteGenerator instance;

    public static SpriteGenerator Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<SpriteGenerator>();
            return instance;
        }
    }

    // Load sprite from a file with given path
    public Sprite LoadSprite(string path, float PPU = 100.0f)
    {
        Sprite newSprite;
        Texture2D spriteTexture = LoadTexture(path);
        newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), PPU);

        return newSprite;
    }

    public Texture2D LoadTexture(string path)
    {
        Texture2D texture;
        byte[] data;

        if(File.Exists(path))
        {
            data = File.ReadAllBytes(path);
            texture = new Texture2D(2, 2);
            if (texture.LoadImage(data))
                return texture;
        }
        return null;
    }
}
