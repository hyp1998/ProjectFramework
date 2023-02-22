using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : SingletonMonoBehaviour<ResourcesManager>
{

    public Dictionary<ResType, string> resPath = new Dictionary<ResType, string>()
    {
        { ResType.UI,"Prefabs/UI/"}
    };

    public T LoadRes<T>(ResType resType, string name) where T : UnityEngine.Object
    {
        string resPath = GetResPathPrefix(resType) + name;
        T obj = Resources.Load<T>(resPath);
        if (obj == null)
        {
            Debug.LogError($"资源未找到：{resPath}");
            return null;
        }
        return obj;
    }

    IEnumerator AsyncLoadResources<T>(ResType resType, string name, Action<T> callBack = null) where T : UnityEngine.Object
    {
        string resPath = GetResPathPrefix(resType) + name;
        ResourceRequest resourcesRequest = Resources.LoadAsync<T>(resPath);
        while (!resourcesRequest.isDone)
        {
            yield return null;
        }
        callBack?.Invoke(resourcesRequest.asset as T);
    }

    public string GetResPathPrefix(ResType resType)
    {
        string path;
        if (resPath.TryGetValue(resType, out path))
        {
            return path;
        }
        return "";
    }


}

public enum ResType
{
    UI,
    Character,
    HeadIcon,
    UI_BGStyle,
}