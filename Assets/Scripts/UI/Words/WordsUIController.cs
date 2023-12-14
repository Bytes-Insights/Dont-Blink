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

    private VisualElement Title;
    private VisualElement Word_IAm;
    private VisualElement Word_Not;
    private VisualElement Word_Afraid;

    private bool shown = false;

    // Start is called before the first frame update
    void Start()
    {
        Title = wordsDocument.rootVisualElement.Q("CollectedWords");
        Title.style.backgroundImage = new StyleBackground(Texture_Title);

        Word_IAm = wordsDocument.rootVisualElement.Q("IAM");
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_QuestionMarks);

        Word_Not = wordsDocument.rootVisualElement.Q("NOT");
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_QuestionMarks);

        Word_Afraid = wordsDocument.rootVisualElement.Q("AFRAID");
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_QuestionMarks);
    }

    public void setIamWord()
    {
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_IAm);
    }

    public void setNotWord()
    {
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_Not);
    }

    public void setAfraidWord()
    {
        Word_IAm.style.backgroundImage = new StyleBackground(Texture_Afraid);
    }

    public void show(bool show)
    {
        if(shown != show)
        {
            if(shown)
            {
            shown = false;
            wordsDocument.enabled = false;
            }else{
                shown = true;
                wordsDocument.enabled = true;
            }
        } 
    }

}
