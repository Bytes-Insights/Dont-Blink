using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class WordsUIController : MonoBehaviour
{
    public UIDocument wordsDocument; 

    public Texture2D Texture_Title;
    public Texture2D Texture_IAm;
    public Texture2D Texture_Not;
    public Texture2D Texture_Afraid;
    public Texture2D Texture_QuestionMarks;

    private VisualElement root;
    private VisualElement Title;
    private VisualElement Word_IAm;
    private VisualElement Word_Not;
    private VisualElement Word_Afraid;

    private bool shown = false;

    // Start is called before the first frame update
    void Start()
    {
        root = wordsDocument.rootVisualElement.Q("Menu");

        Title = wordsDocument.rootVisualElement.Q("CollectedWords");
        Title.style.backgroundImage = new StyleBackground(Texture_Title);

        Word_IAm = wordsDocument.rootVisualElement.Q("IAM");
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_QuestionMarks);

        Word_Not = wordsDocument.rootVisualElement.Q("NOT");
        Word_Not.style.backgroundImage = new StyleBackground(Texture_QuestionMarks);

        Word_Afraid = wordsDocument.rootVisualElement.Q("AFRAID");
        Word_Afraid.style.backgroundImage = new StyleBackground(Texture_QuestionMarks);

        root.visible = false;
    }

    public void setIamWord()
    {
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_IAm);
    }

    public void setNotWord()
    {
        Word_Not.style.backgroundImage = new StyleBackground(Texture_Not);
    }

    public void setAfraidWord()
    {
        Word_Afraid.style.backgroundImage = new StyleBackground(Texture_Afraid);
    }

    public void show(bool show)
    {
        if(shown != show)
        {
            if(shown)
            {
                shown = false;
                root.visible = false;
            }else{
                shown = true;
                root.visible = true;
            }
        } 
    }

}
