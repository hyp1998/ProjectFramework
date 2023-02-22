using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CarouselCom : MonoBehaviour
{
    public bool isStartRun = false;
    /// <summary> 本地图片 </summary>
    public List<Sprite> localSpriteList;
    /// <summary> 网络图片Url </summary>
    public List<string> netSpriteList;
    /// <summary> 停留时间 </summary>
    public List<float> stayTimeList;
    /// <summary> 透明度 </summary>
    public List<float> opacityList;

    public RectTransform item;
    public Transform parent;
    public LoadType loadType = LoadType.Net;
    public Vector2 size = new Vector2(500, 500);

    private int index = 0;
    private Vector3 lastPos;
    /// <summary> 实例化的item数量 </summary>
    private int itemCount = 2;
    /// <summary> 要展示的图片数量 </summary>
    private int ItemListCount = 0;

    private int DownLoadIndex = 0;

    private List<CarouselItem> itemList = new List<CarouselItem>();

    void Start()
    {
        if (isStartRun)
        {
            Init();
        }
    }

    public void Init()
    {
        ItemListCount = Count();
        DefaultTimerValue();
        DefaultOpacityValue();
        Create();
        StartCoroutine(AutoCarousel());
    }

    private void DefaultOpacityValue()
    {
        //没赋值默认透明度1
        if (opacityList == null || opacityList.Count <= 0)
        {
            for (int i = 0; i < ItemListCount; i++)
            {
                opacityList.Add(1f);
            }
        }
    }

    private void DefaultTimerValue()
    {
        //没赋值默认2秒
        if (stayTimeList == null || stayTimeList.Count <= 0)
        {
            for (int i = 0; i < ItemListCount; i++)
            {
                stayTimeList.Add(2);
            }
        }
    }

    public void SetCarouselComSize(Vector2 _size)
    {
        size = _size;
    }

    public void SetLocalSprite(List<Sprite> _localSpriteList)
    {
        localSpriteList = new List<Sprite>(_localSpriteList);
    }

    public void SetNetSpriteUrl(List<string> netsprites)
    {
        netSpriteList = new List<string>(netsprites);
    }

    public void SetStayTime(List<float> _stayTimeList)
    {
        stayTimeList = new List<float>(_stayTimeList);
    }

    public void SetOpacity(List<float> _opacityList)
    {

        if (opacityList.Count > 0) opacityList.Clear();
        opacityList = new List<float>(_opacityList);
    }



    private void Create()
    {
        SetSize(size);
        itemList.Clear();
        lastPos = new Vector3(item.rect.width * (itemCount - 1), 0, 0);
        for (int i = 0; i < itemCount; i++)
        {
            if (i >= Count()) return;

            GameObject obj = Instantiate(item.gameObject);
            obj.SetActive(true);
            obj.transform.SetParent(parent, false);

            CarouselItem it = obj.GetComponent<CarouselItem>();
            it.Init(size, lastPos, i, Count(), itemCount);
            it.SetPos(new Vector3(item.rect.width * i, 0, 0));
            SetSprite(it);

            itemList.Add(it);
        }
    }

    private IEnumerator AutoCarousel()
    {
        if (Count() <= 1) yield break;
        float time;
        while (true)
        {
            if (index >= ItemListCount) index = 0;
            time = index < stayTimeList.Count ? stayTimeList[index] : 2;
            yield return new WaitForSeconds(time);
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].Move(item.rect.width, (item) =>
                {
                    SetSprite(item);
                });
            }
            index++;
        }
    }

    private void SetSize(Vector2 size)
    {
        this.transform.GetComponent<RectTransform>().sizeDelta = size;
        item.sizeDelta = size;
        parent.transform.GetComponent<RectTransform>().sizeDelta = size;
    }

    private int Count()
    {
        switch (loadType)
        {
            case LoadType.Local:
                return localSpriteList.Count;
            case LoadType.Net:
                return netSpriteList.Count;
        }
        return 0;
    }

    private void SetSprite(CarouselItem item)
    {
        switch (loadType)
        {
            case LoadType.Local:

                if (item.spIndex < localSpriteList.Count)
                    item.SetSprite(localSpriteList[item.spIndex], opacityList[item.spIndex]);

                break;
            case LoadType.Net:

                if (item.spIndex < localSpriteList.Count)
                    item.SetSprite(netSpriteList[item.spIndex], opacityList[item.spIndex]);

                break;
        }
    }




    public void StartDownLoadTexOrLocal(List<string> netsprites)
    {
        loadType = LoadType.Local;
        SetNetSpriteUrl(netsprites);
        DownLoadIndex = 0;
        DownLoadAllImageUrl();
    }

    public void DownLoadAllImageUrl()
    {
        CSNetworking.GetAsync(netSpriteList[DownLoadIndex], (data) =>
        {
            byte[] urlContents = data[0] as byte[];
            localSpriteList.Add(Utility.GetSprite(urlContents));
            DownLoadIndex++;
            if (DownLoadIndex >= netSpriteList.Count)
            {
                gameObject.SetActive(true);
                Init();
                return;
            }
            DownLoadAllImageUrl();
        }, RequestType.ByteArray);
    }

    public void RestCom()
    {
        localSpriteList.Clear();
        netSpriteList.Clear();
        stayTimeList.Clear();
        opacityList.Clear();
    }


}

public enum LoadType
{
    Local,
    Net,
}