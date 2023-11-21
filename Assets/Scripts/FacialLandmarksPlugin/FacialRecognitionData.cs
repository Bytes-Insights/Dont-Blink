using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialRecognitionData
{
    private bool successful;
    private List<List<Vector2>> faces = new List<List<Vector2>>();

    public void Reset()
    {
        this.successful = false;
        this.faces.Clear();
    }

    public bool IsSuccessful()
    {
        return this.successful;
    }

    public void SetSuccessful(bool successful)
    {
        this.successful = successful;
    }

    public List<Vector2> AddFace()
    {
        List<Vector2> face = new List<Vector2>();
        faces.Add(face);
        return face;
    }

    public int GetFaceCount()
    {
        return this.faces.Count;
    }

    public List<Vector2> GetFace(int i)
    {
        return this.faces[i];
    }

    public void CopyFrom(FacialRecognitionData other)
    {
        this.Reset();
        this.SetSuccessful(other.IsSuccessful());
        for (int i = 0; i < other.GetFaceCount(); i++)
        {
            var otherFace = other.GetFace(i);
            var face = AddFace();
            for (int j = 0; j < otherFace.Count; j++)
            {
                face.Add(otherFace[j]);
            }
        }
    }
}