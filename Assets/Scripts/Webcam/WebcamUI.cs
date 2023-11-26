using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WebcamUI : MonoBehaviour
{
    private const string WEBCAM = "Webcam";

    public WebcamSupplier supplier;
    public UIDocument document;

    private Texture2D uiTexture;
    private bool set = false;

    void Update()
    {
        if (set)
        {
            Graphics.CopyTexture(supplier.GetWebCamTexture(), uiTexture);
            Debug.Log("Bill");
            return;
        }

        if (document != null && supplier != null && supplier.GetWebCamTexture() != null)
        {
            set = true;
            WebCamTexture wcTex = supplier.GetWebCamTexture();
            uiTexture = new Texture2D(wcTex.width, wcTex.height, TextureFormat.ARGB32, 1, false);

            document.rootVisualElement.Q<VisualElement>(WEBCAM).style.backgroundImage = new StyleBackground(uiTexture);
            Debug.Log("Bill");
        }
    }
}
