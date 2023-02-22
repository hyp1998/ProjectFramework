using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Expand
{
    public static void SetWebUIPos(this Transform thisTran, Vector2 pos)
    {
        RectTransform rectTransform = thisTran.GetComponent<RectTransform>();
        if (rectTransform)
        {
            rectTransform.pivot = new Vector2(0, 1);
            thisTran.transform.localPosition = Utility.WebOrUnityPos(pos.x, pos.y);
        }
    }

    public static void SetWebUIPos(this Transform thisTran, float x, float y)
    {
        RectTransform rectTransform = thisTran.GetComponent<RectTransform>();
        if (rectTransform)
        {
            rectTransform.pivot = new Vector2(0, 1);
            thisTran.transform.localPosition = Utility.WebOrUnityPos(x, y);
        }
    }

    public static void SetWebUIPos(this RectTransform rectTransform, Vector2 pos)
    {
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.transform.localPosition = Utility.WebOrUnityPos(pos.x, pos.y);
    }

    public static void SetWebUIPos(this RectTransform rectTransform, float x, float y)
    {
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.transform.localPosition = Utility.WebOrUnityPos(x, y);
    }

    public static string ForwardSlash(this string path)
    {
        if (path == null)
            return null;
        return path.Replace('\\', '/');
    }

    public static TValue Parse<TValue>(this System.Enum value)
    {
        return (TValue)(object)value;
    }

    public static string RemovePlaceholder_HY(this string str)
    {
        return str.Replace("#IMG#", "");
    }

    public static T Get<T>(this Transform transform, string path) where T : UnityEngine.Object
    {
        if (transform == null)
            return null;
        return Utility.GetObject<T>(transform, path);
    }

}
