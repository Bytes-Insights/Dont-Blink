using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WebcamUIWithPoints : MonoBehaviour
{
    private const string WEBCAM = "Webcam";

    public WebcamSupplier supplier;
    public UIDocument document;
    public BlinkModality modality;
    public InteractionMediator mediator;

    private VisualElement elem;
    private Texture2D uiTexture;
#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
    private Texture2D uiTextureRGB;
#endif
    private Texture2D indicatorTexture;
    private Texture2D activeIndicatorTexture;
    private bool set = false;

    private float minRecorded = 999F;
    private float maxRecorded = 0F;

    void Start()
    {
        indicatorTexture = new Texture2D(1, 1);
        indicatorTexture.SetPixel(0, 0, new Color(1, 1, 0));
        indicatorTexture.Apply();

        activeIndicatorTexture = new Texture2D(1, 1);
        activeIndicatorTexture.SetPixel(0, 0, new Color(1, 0, 0));
        activeIndicatorTexture.Apply();
    }

    void DrawRectangle(Rect rect, bool red)
    {
        if (red)
        {
            GUI.skin.box.normal.background = activeIndicatorTexture;

        } else
        {
            GUI.skin.box.normal.background = indicatorTexture;

        }
        GUI.Box(rect, GUIContent.none);
    }

    private void OnGUI()
    {
        float left = Screen.width - 30F - 320F;
        float top = Screen.height - 30F - 240F;

        if (set)
        {
            float lastEAR = modality.GetLastEAR();
            if (lastEAR > maxRecorded)
            {
                maxRecorded = lastEAR;
            }

            if (lastEAR < minRecorded && lastEAR != 0F) {
                minRecorded = lastEAR;
            }

            GUIStyle style = new GUIStyle();
            style.fontSize = 24;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = new Color(1, 1, 1);
            GUI.TextArea(new Rect(new Vector2(left, top - 40), new Vector2(320F, 30)), "last: " + (Mathf.Round(lastEAR * 10) / 10) +
                " / min: " + (Mathf.Round(minRecorded * 10) / 10) + " / max: " + (Mathf.Round(maxRecorded * 10) / 10), style);
            FacialRecognitionData data = modality.GetResultData();

            for (int i = 0; i < data.GetFaceCount(); i++)
            {
                List<Vector2> face = data.GetFace(i);
                for (int j = 0; j < face.Count; j++)
                {
                    Vector2 poi = face[j];

                    float x = poi.x * 0.5F + left;
                    float y = poi.y * 0.5F + top;
                    float size = 2;

                    Rect rect = new Rect(new Vector2(x - size, y - size), new Vector2(size * 2, size * 2));

                    if (mediator && mediator.EyesClosed())
                    {
                        DrawRectangle(rect, true);
                    } else
                    {
                        DrawRectangle(rect, false);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (set)
        {
            WebCamTexture wcTex = supplier.GetWebCamTexture();
            if (wcTex.width == 640 && wcTex.height == 480) {
                Graphics.CopyTexture(supplier.GetWebCamTexture(), uiTexture);

#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
                uiTexture.Apply();
                uiTextureRGB.SetPixels32(uiTexture.GetPixels32());
                uiTextureRGB.Apply();
#endif 

            }
            return;
        }

        if (document != null && supplier != null && supplier.GetWebCamTexture() != null)
        {
            set = true;
            WebCamTexture wcTex = supplier.GetWebCamTexture();

#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
            uiTexture = new Texture2D(640, 480, TextureFormat.BGRA32, 1, false);
            uiTextureRGB = new Texture2D(640, 480, TextureFormat.RGB24, 1, false);
#else
            uiTexture = new Texture2D(wcTex.width, wcTex.height, TextureFormat.ARGB32, 1, false);
#endif

            elem = document.rootVisualElement.Q<VisualElement>(WEBCAM);

#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
            elem.style.backgroundImage = new StyleBackground(uiTextureRGB);
#else
            elem.style.backgroundImage = new StyleBackground(uiTexture);
#endif
        }
    }
}
