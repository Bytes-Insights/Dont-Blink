using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonProvider : MonoBehaviour
{

    private const string AButton = "AButton";

    public UIDocument document;
    public Texture2D AButtonTexture;
    private Texture2D uiTexture;

    void Start()
    {
        if (document != null && AButtonTexture != null)
        {
            uiTexture = new Texture2D(AButtonTexture.width, AButtonTexture.height, TextureFormat.ARGB32, 1, false);
            document.rootVisualElement.Q<VisualElement>(AButton).style.backgroundImage = new StyleBackground(AButtonTexture);
        }
    }

}
