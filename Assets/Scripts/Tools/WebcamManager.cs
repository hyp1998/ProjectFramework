using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class WebcamManager
{
    private static WebCamTexture _webcamTexture;
    public static WebCamTexture Texture
    {
        get
        {
            if (_webcamTexture == null)
            {
                Initialize();
            }
            return _webcamTexture;
        }
    }
    public static bool IsPlaying => Texture.isPlaying;

    public static void Play()
    {
        Texture.Play();
    }

    public static void Pause()
    {
        Texture.Pause();
    }

    public static void Stop()
    {
        Texture.Stop();
    }

    public static IEnumerator RequestCameraAuthorization()
    {
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Initialize();
        }
    }

    public static WebCamTexture CreateNewWebCamTexture(bool shouldUseFrontFacing = true)
    {
        WebCamTexture webCamTexture = null;

        for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
        {
            var device = WebCamTexture.devices[cameraIndex];
            Debug.Log("cameraIndex: " + cameraIndex + ", is frontFace: " + device.isFrontFacing);
            if (device.isFrontFacing == shouldUseFrontFacing)
            {
                webCamTexture = new WebCamTexture(device.name);//, requestedWidth, requestedHeight, requestedFPS);
                                                               //useCamera = true;
                break;
            }
        }

        if (webCamTexture == null)
        {
            Debug.Log("webCamTexture is null");
            if (WebCamTexture.devices.Length > 0)
            {
                webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);//, requestedWidth, requestedHeight, requestedFPS);
            }
            else
            {
                webCamTexture = new WebCamTexture();//requestedWidth, requestedHeight);
            }
        }

        return webCamTexture;
    }


    public static void Initialize(bool shouldUseFrontFacing = true)//int requestedWidth, int requestedHeight, int requestedFPS,
    {
        if (_webcamTexture != null)
        {
            UnityEngine.Object.DestroyImmediate(_webcamTexture);
        }

        _webcamTexture = CreateNewWebCamTexture(shouldUseFrontFacing);
    }
}
