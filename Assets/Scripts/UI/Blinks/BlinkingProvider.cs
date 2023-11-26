using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlinkingProvider : MonoBehaviour
{

    private const string Eye = "Eye";
    private const string BlinkLabel = "BlinksLabel";
    private int count = 0;

    public UIDocument document;
    public Texture2D OpenEyeTexture;
    public Texture2D ClosedEyeTexture;
    private Texture2D uiTexture;

    void Start()
    {
        if (document != null && OpenEyeTexture != null && ClosedEyeTexture != null)
        {
            document.rootVisualElement.Q<VisualElement>(Eye).style.backgroundImage = new StyleBackground(OpenEyeTexture);
            document.rootVisualElement.Q<Label>(BlinkLabel).text = count.ToString();
        }
    }

    public void closeEye(){
        document.rootVisualElement.Q<VisualElement>(Eye).style.backgroundImage = new StyleBackground(ClosedEyeTexture);
    }

    public void openEye(){
        count++;
        document.rootVisualElement.Q<Label>(BlinkLabel).text = count.ToString();
        document.rootVisualElement.Q<VisualElement>(Eye).style.backgroundImage = new StyleBackground(OpenEyeTexture);
    }
}
