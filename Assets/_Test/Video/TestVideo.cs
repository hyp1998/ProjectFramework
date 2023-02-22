using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TestVideo : MonoBehaviour
{
    public VideoPlayer vp;
    public Button testBtn;

    void Start()
    {
        vp.clip = Resources.Load<VideoClip>("VideoTest");
        vp.isLooping = true;
        vp.Play();
        testBtn.onClick.AddListener(()=>{ Debug.LogError("Œ“ «≤‚ ‘µƒ"); });
    }

}
