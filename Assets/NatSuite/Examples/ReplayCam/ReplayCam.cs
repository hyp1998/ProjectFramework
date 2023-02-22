/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using UnityEngine;
    using System.Collections;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;
    using UnityEngine.UI;

    public class ReplayCam : MonoBehaviour {

        [Header(@"Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;

        private IMediaRecorder recorder;
        private CameraInput cameraInput;
        private AudioInput audioInput;
        private AudioSource microphoneSource;

        private IEnumerator Start () {
            // Start microphone
            microphoneSource = gameObject.AddComponent<AudioSource>();
            microphoneSource.mute =
            microphoneSource.loop = true;
            microphoneSource.bypassEffects =
            microphoneSource.bypassListenerEffects = false;
            microphoneSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
            yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
            microphoneSource.Play();
        }

        private void OnDestroy () {
            // Stop microphone
            microphoneSource.Stop();
            Microphone.End(null);
        }

        public void StartRecording () {
            // Start recording
            var frameRate = 30;
            var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
            var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
            var clock = new RealtimeClock();
            recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount);
            // Create recording inputs
            cameraInput = new CameraInput(recorder, clock, Camera.main);
            audioInput = recordMicrophone ? new AudioInput(recorder, clock, microphoneSource, true) : null;
            // Unmute microphone
            microphoneSource.mute = audioInput == null;
        }

        public async void StopRecording () {
            // Mute microphone
            microphoneSource.mute = true;
            // Stop recording
            audioInput?.Dispose();
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            // Playback recording
            Debug.Log($"Saved recording to: {path}");
            Handheld.PlayFullScreenMovie($"file://{path}");
        }

        RawImage Img_Selfie;
        byte[] Img_SelfieDatas;

        public void PhotoGraph()
        {
            WebcamManager.Texture.Play();
            Img_Selfie.texture = WebcamManager.Texture;
            float w = WebcamManager.Texture.width;
            float h = WebcamManager.Texture.height;
            float w1 = 0f;
            float h1 = 0f;
            if (Application.platform == RuntimePlatform.Android)
            {
                Img_Selfie.transform.localEulerAngles = new Vector3(0, 0, 90);
                w1 = Screen.height;
                h1 = h / (w / Screen.height);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Img_Selfie.transform.localEulerAngles = new Vector3(0, 0, -90);
                w1 = Screen.height;
                h1 = h / (w / Screen.height);
            }
            else
            {
                Img_Selfie.transform.localEulerAngles = new Vector3(0, 0, 0);
                w1 = w / (h / Screen.height);
                h1 = Screen.height;
            }
            Img_Selfie.rectTransform.sizeDelta = new Vector2(w1, h1);

            if (Application.platform == RuntimePlatform.Android)
            {
                Img_SelfieDatas = Utility.GetTextureByte(WebcamManager.Texture, false);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Img_SelfieDatas = Utility.GetTextureByte(WebcamManager.Texture, true);
            }
            else
            {
                Img_SelfieDatas = Utility.GetTextureByte(WebcamManager.Texture, false);
            }

            //用完记得结束掉
            WebcamManager.Texture.Stop();
        }
    }
}