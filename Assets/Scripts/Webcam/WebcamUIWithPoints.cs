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
            Graphics.CopyTexture(supplier.GetWebCamTexture(), uiTexture);
            return;
        }

        if (document != null && supplier != null && supplier.GetWebCamTexture() != null)
        {
            set = true;
            WebCamTexture wcTex = supplier.GetWebCamTexture();
            uiTexture = new Texture2D(wcTex.width, wcTex.height, TextureFormat.ARGB32, 1, false);

            elem = document.rootVisualElement.Q<VisualElement>(WEBCAM);
            elem.style.backgroundImage = new StyleBackground(uiTexture);

            Debug.Log(elem.WorldToLocal(new Vector3(0, 0)));
            Debug.Log(elem.LocalToWorld(new Vector3(0, 0)));
            Debug.Log(elem.style.position);
            Debug.Log(elem.layout);
            Debug.Log(elem.localBound);
            Debug.Log(elem.worldBound);
            Debug.Log(elem.transform.position);
            Debug.Log(elem.contentRect.x + "/ " + elem.contentRect.y);
        }
    }
}
