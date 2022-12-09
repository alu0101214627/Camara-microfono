using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tvScript : MonoBehaviour
{
    static WebCamTexture backCamera;
    // Start is called before the first frame update
    private string savePath = "C:/Users/Jorge/Desktop/Universidad/Unity/Microfono y camara/Assets/Snapshots/capture";
    private int captureCounter = 0;
    IEnumerator Start()
    {   
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (backCamera == null)
                backCamera = new WebCamTexture();

            GetComponent<Renderer>().material.mainTexture = backCamera;

            Debug.Log("Nombre de la cámara: " + WebCamTexture.devices[0]);

            if (!backCamera.isPlaying)
                backCamera.Play();
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            if (!Microphone.IsRecording(Microphone.devices[0]))
                Microphone.Start(Microphone.devices[0], true, 10, 44100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            if (!backCamera.isPlaying) {
                TakeSnapshot();
            } else {
                TakeSnapshot();
            }
        }
        if (Input.GetKeyDown("backspace")) {
            if (!Microphone.IsRecording(Microphone.devices[0])) {
                Microphone.Start(Microphone.devices[0], true, 10, 44100);
            } else {
                Microphone.End(Microphone.devices[0]);
            }
        }
    }
    void TakeSnapshot()
    {
        Texture2D snap = new Texture2D(backCamera.width, backCamera.height);
        snap.SetPixels(backCamera.GetPixels());
        snap.Apply();
        
        System.IO.File.WriteAllBytes(savePath + captureCounter.ToString() + ".png", snap.EncodeToPNG());
        ++captureCounter;
    }
}
