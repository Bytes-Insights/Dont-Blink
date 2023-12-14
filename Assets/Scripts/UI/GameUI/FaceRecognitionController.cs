using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FaceRecognitionController : MonoBehaviour
{

    public UIDocument document; 

    public Texture2D Texture_OK;
    public Texture2D Texture_NO;

    private VisualElement face;

    // Start is called before the first frame update
    void Start()
    {
        face = document.rootVisualElement.Q("FaceRecognition");
    }

    public void updateFace(bool recognised)
    {
        if(recognised)
        {
            face.style.backgroundImage = new StyleBackground(Texture_OK);
        }else{
            face.style.backgroundImage = new StyleBackground(Texture_NO);
        }
    }
}
