using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamSupplier : MonoBehaviour
{
    private WebCamTexture webCamTex;

    void Start()
    {
        webCamTex = new WebCamTexture(WebCamTexture.devices[0].name, 640, 480, 30);
        webCamTex.Play();
    }

    public WebCamTexture GetWebCamTexture()
    {
        return webCamTex;
    }

    void OnDestroy()
    {
        if(webCamTex != null)
        {
            webCamTex.Stop();
        }
    }
}
