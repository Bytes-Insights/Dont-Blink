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
        if (set )
        {
            WebCamTexture wcTex = supplier.GetWebCamTexture();
            if(wcTex.width==640 && wcTex.height==480){
            Graphics.CopyTexture(supplier.GetWebCamTexture(), uiTexture);
            }
            return;
        }

        if (document != null && supplier != null && supplier.GetWebCamTexture() != null)
        {
            set = true;
            WebCamTexture wcTex = supplier.GetWebCamTexture();
            uiTexture = new Texture2D(640, 480, TextureFormat.BGRA32, 1, false);
            
			uiTexture.Apply();
            document.rootVisualElement.Q<VisualElement>(WEBCAM).style.backgroundImage = new StyleBackground(uiTexture);
        }
    }
}
