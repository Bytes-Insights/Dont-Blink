using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using UnityEngine;

public class BlinkModality : MonoBehaviour
{
    // Public inputs.
    public InteractionMediator mediator;
    public WebcamSupplier camSupplier;

    // Textures.
    private Texture2D sendableTexture;

#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
    private Texture2D mediumTexture;
#endif

    // Result state.
    private FacialRecognitionData resultData = new FacialRecognitionData();
    private float lastEAR = 0.0F;

    // Data exchange for threads.
    private byte[] textureData;
    private int textureWidth;
    private int textureHeight;
    private FacialRecognitionData cv_data = new FacialRecognitionData();

    // Control variables for threads.
    private volatile int lastTextureUpdate = 0;
    private volatile int lastTextureRetrieve = 0;
    private volatile int lastResultUpdate = 0;
    private volatile int lastResultRetrieve = 0;
    private bool threadRunning = true;

    // Mutexes.
    private Mutex textureMutex = new Mutex();
    private Mutex resultMutex = new Mutex();

    //Sensitivity
    private float sensitivity = 4.2f;

    // --- EXTRA THREAD ---

    void ComputerVisionThread()
    {
        while (threadRunning)
        {
            if (lastTextureUpdate <= lastTextureRetrieve)
            {
                Thread.Yield();
                continue;
            }


            byte[] texBytes;
            int texWidth, texHeight;

            textureMutex.WaitOne();

            texBytes = textureData;
            texWidth = textureWidth;
            texHeight = textureHeight;
            lastTextureRetrieve = lastResultUpdate;

            textureMutex.ReleaseMutex();

            FacialLandmarksPlugin.Result result = FacialLandmarksPlugin.UpdateFacialLandmarks(texBytes, texWidth, texHeight);

            FacialRecognitionData currData = new FacialRecognitionData();

            if (result.size != result.faceAmount * 68)
            {
                return;
            }

            currData.SetSuccessful(result.successful);

            IntPtr ptr = result.data;
            int structSize = Marshal.SizeOf(typeof(FacialLandmarksPlugin.OutPoint));

            for (int i = 0; i < (int)result.faceAmount; i++)
            {
                List<Vector2> face = currData.AddFace();

                for (int j = 0; j < 68; j++)
                {
                    FacialLandmarksPlugin.OutPoint point = Marshal.PtrToStructure<FacialLandmarksPlugin.OutPoint>(ptr);
                    face.Add(new Vector2(point.x, point.y));
                    ptr += structSize;
                }
            }

            resultMutex.WaitOne();

            cv_data = currData;
            lastResultUpdate++;

            resultMutex.ReleaseMutex();
        }
    }

    // --- MAIN THREAD --- 

    void InitializeCVThread()
    {
        var thread = new Thread(ComputerVisionThread);
        thread.Start();
    }

    void Awake()
    {
        if(DataTransfer.instance != null && DataTransfer.instance.valueToPass != null){
            sensitivity = DataTransfer.instance.valueToPass;
            Debug.Log(sensitivity);
        }
    }

    void Start()
    {
        FacialLandmarksPlugin.InitializeDLIB(Application.streamingAssetsPath + "/shape_predictor_68_face_landmarks.dat\0");
        InitializeCVThread();
    }

    void OnDestroy()
    {
        threadRunning = false;
    }

    void UpdateSendableTexture()
    {
        if (sendableTexture == null)
        {
            sendableTexture = new Texture2D(640, 480, TextureFormat.RGB24, false);
            sendableTexture.Apply();

#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
            mediumTexture = new Texture2D(640, 480, TextureFormat.BGRA32, false);
            mediumTexture.Apply();
#endif
        }


        if (camSupplier == null)
        {
            Debug.LogWarning("No cam supplier found!");
            return;
        }


        WebCamTexture webCamTex = camSupplier.GetWebCamTexture();

        if (webCamTex == null)
        {
            Debug.LogWarning("No webcam texture found!");
            return; 
        }


        if (webCamTex.width > 100)
        {
#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
            Graphics.CopyTexture(webCamTex, mediumTexture);
            mediumTexture.Apply();
            sendableTexture.SetPixels32(mediumTexture.GetPixels32());
            sendableTexture.Apply();
#else
            sendableTexture.SetPixels32(webCamTex.GetPixels32());
#endif 
        }
    }

    void ProcessSendableTexture(Texture2D tex)
    {
        textureMutex.WaitOne();

        textureData = tex.GetRawTextureData();
        textureWidth = tex.width;
        textureHeight = tex.height;
        lastTextureUpdate++;

        textureMutex.ReleaseMutex();

        if (lastResultUpdate <= lastResultRetrieve)
        {
            return;
        }

        resultMutex.WaitOne();

        lastResultRetrieve = lastResultUpdate;
        resultData.CopyFrom(cv_data);

        resultMutex.ReleaseMutex();
    }

    private Vector2 midpoint(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }

    private float dist(Vector2 p1, Vector2 p2)
    {
        float dx = p1.x - p2.x;
        float dy = p1.y - p2.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    private float blinkRatio(int[] eyePoints, List<Vector2> face)
    {
        Vector2 cornerLeft = new Vector2(face[eyePoints[0]].x, face[eyePoints[0]].y);
        Vector2 cornerRight = new Vector2(face[eyePoints[3]].x,face[eyePoints[3]].y);
        Vector2 centerTop = midpoint(face[eyePoints[1]], face[eyePoints[2]]);
        Vector2 centerBottom = midpoint(face[eyePoints[5]], face[eyePoints[4]]);

        float horizontalLength = dist(cornerLeft, cornerRight);
        float verticalLength = dist(centerTop, centerBottom);

        float ratio = horizontalLength / verticalLength;
        return ratio;
    }

    void EvalData()
    {
        if (!resultData.IsSuccessful() || resultData.GetFaceCount() == 0)
        {
            if (mediator.FaceTracked())
            {
                mediator.SetFaceTracked(false);
            }

            if (mediator.EyesClosed())
            {
                mediator.SetEyesStatus(false);
            }

            return;
        }

        if (!mediator.FaceTracked())
        {
            mediator.SetFaceTracked(true);
        }

        int[] leftEyeLandmarks = { 36, 37, 38, 39, 40, 41 };
        int[] rightEyeLandmarks = { 42, 43, 44, 45, 46, 47 };

        bool rec = false;
        float faceDirectionalWeight = 0F;
        for (int i = 0; i < resultData.GetFaceCount(); i++)
        {
            List<Vector2> face = resultData.GetFace(i);

            Vector2 faceAnchor = midpoint(new Vector2(face[27].x, face[27].y), new Vector2(face[8].x, face[8].y));
            float normalizedAnchorX = faceAnchor.x - 320;
            faceDirectionalWeight = normalizedAnchorX / 320;

            float leftEyeRatio = blinkRatio(leftEyeLandmarks, face);
            float rightEyeRatio = blinkRatio(rightEyeLandmarks, face);
            float ratio = (leftEyeRatio + rightEyeRatio) / 2;
            lastEAR = ratio;
            if (ratio > sensitivity)
            {
                rec = true;
                break;
            }
        }

        mediator.SetFaceDirectionalWeight(faceDirectionalWeight);

        if (mediator.EyesClosed() != rec)
        {
            mediator.SetEyesStatus(rec);
        }
    }


    void Update()
    {
        UpdateSendableTexture();
        ProcessSendableTexture(sendableTexture);
        EvalData();
    }

    public FacialRecognitionData GetResultData()
    {
        return resultData;
    }

    public float GetLastEAR()
    {
        return lastEAR;
    }

    public void updateSensitivity(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }
}
