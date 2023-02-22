using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;


public class Utility
{

    /// <summary>
    /// 是否横屏
    /// </summary>
    /// <returns></returns>
    public bool IsLandscape()
    {
        return Screen.width > Screen.height;
    }

    public static Sprite GetSprite(Byte[] bytes)
    {
        try
        {
            UnityEngine.Texture2D texture = new UnityEngine.Texture2D(10, 10);
            texture.LoadImage(bytes);
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return sp;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    public static Texture2D GetTexture2D(Byte[] bytes)
    {
        try
        {
            UnityEngine.Texture2D texture = new UnityEngine.Texture2D(10, 10);
            texture.LoadImage(bytes);
            return texture;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    public static T DeepCopy<T>(T obj)
    {
        if (obj is string || obj.GetType().IsValueType) return obj;

        object retval = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (FieldInfo field in fields)
        {
            try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
            catch { }
        }
        return (T)retval;
    }

    public static long GetTimeMilliseconds()
    {
        //精确到毫秒
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }

    public static Vector2 WebOrUnityPos(Vector2 webpos)
    {
        return new Vector2(webpos.x - (Screen.width / 2), -webpos.y + (Screen.height / 2));
    }

    public static Vector2 WebOrUnityPos(float x, float y)
    {
        return new Vector2(x - (Screen.width / 2), -y + (Screen.height / 2));
    }

    public static T GetObject<T>(Transform parent, string path) where T : UnityEngine.Object
    {
        if (parent == null) return null;

        Transform transform = parent.Find(path);
        if (transform == null) return null;

        if (typeof(T) == typeof(Transform)) return transform as T;

        if (typeof(T) == typeof(GameObject)) return transform.gameObject as T;

        return transform.GetComponent(typeof(T)) as T;
    }

    public static T GetObject<T>(GameObject go, string path) where T : UnityEngine.Object
    {
        if (go == null) return null;
        return GetObject<T>(go.transform, path);
    }


    public static T Get<T>(Transform parent, string name, ref T obj) where T : UnityEngine.Object
    {
        return obj ?? (obj = GetObject<T>(parent, name));
    }

    public static T Get<T>(GameObject go, ref T component) where T : Component
    {
        if (!component)
        {
            component = go.GetComponent<T>();
            if (!component)
                component = go.AddComponent<T>();
        }
        return component;
    }

    //将RenderTexture保存成一张png图片  
    public static bool SaveRenderTextureToPNG(RenderTexture rt, string contents, string pngName)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(contents))
            Directory.CreateDirectory(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
        return true;

    }

    public static bool BytesToFile(byte[] bytes, string path)
    {
        try
        {
            // 文件存在则删除
            if (System.IO.File.Exists(path)) { System.IO.File.Delete(path); }

            // 写入文件，方式一
            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(bytes, 0, bytes.Length);
                }
            }
            return true;
            // 写入文件，方式二
            //System.IO.File.WriteAllBytes(path, bytes);
        }
        catch
        {
            UnityEngine.Debug.LogError("文件下载失败" + path);
            return false;
        }
    }

    /// <summary>获取秒级别时间戳（10位）</summary>
    public static long GetTimestampToSeconds()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    /// <summary>获取毫秒级别时间戳（13位）</summary>
    public static long GetTimeStampToMilliseconds()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    public static Color GetColorHY(string str)
    {
        string s1 = str.Split('(')[1];
        string s2 = s1.Split(')')[0];
        string[] s3 = s2.Split(',');
        return new Color(float.Parse(s3[0]) / 255f, float.Parse(s3[1]) / 255f, float.Parse(s3[2]) / 255f, float.Parse(s3[3]) / 255f);
    }

    public static bool SaveFile(byte[] bytes, string filePath, string fileNameAndSuffix)
    {
        string path = Path.Combine(filePath, fileNameAndSuffix);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        return BytesToFile(bytes, path);
    }

    public static byte[] GetTextureByte(Texture texture, bool clockwise)
    {
        Texture2D texture2D = TextureToTexture2D(texture);
        byte[] datas = RotateTexture(texture2D, clockwise).EncodeToPNG();
        return datas;
    }

    /// <summary>
    /// 运行模式下Texture转换成Texture2D
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }

    static Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}


public static class MathTools
{
    public static bool IsEqual(float a, float b)
    {
        if (Mathf.Abs(a - b) < 0.000001f)
        {
            return true;
        }
        return false;
    }

    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
    {
        t = Mathf.Clamp01(t);
        Matrix4x4 lerpMatrix = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            lerpMatrix.SetRow(i, Vector4.Lerp(from.GetRow(i), to.GetRow(i), t));
        }
        return lerpMatrix;
    }

    /// <summary>
    /// 对角度进行限制.
    /// </summary>
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}

public enum AnchorPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottonCenter,
    BottomRight,
    BottomStretch,

    VertStretchLeft,
    VertStretchRight,
    VertStretchCenter,

    HorStretchTop,
    HorStretchMiddle,
    HorStretchBottom,

    StretchAll
}

public enum PivotPresets
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight,
}

public static class RectTransformExtensions
{
    public static void SetAnchor(this RectTransform source, AnchorPresets allign, int offsetX = 0, int offsetY = 0)
    {
        source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

        switch (allign)
        {
            case (AnchorPresets.TopLeft):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.TopCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 1);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.TopRight):
                {
                    source.anchorMin = new Vector2(1, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.MiddleLeft):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0.5f);
                    source.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (AnchorPresets.MiddleRight):
                {
                    source.anchorMin = new Vector2(1, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (AnchorPresets.BottomLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 0);
                    break;
                }
            case (AnchorPresets.BottonCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (AnchorPresets.BottomRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.HorStretchTop):
                {
                    source.anchorMin = new Vector2(0, 1);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
            case (AnchorPresets.HorStretchMiddle):
                {
                    source.anchorMin = new Vector2(0, 0.5f);
                    source.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (AnchorPresets.HorStretchBottom):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (AnchorPresets.VertStretchLeft):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (AnchorPresets.VertStretchCenter):
                {
                    source.anchorMin = new Vector2(0.5f, 0);
                    source.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (AnchorPresets.VertStretchRight):
                {
                    source.anchorMin = new Vector2(1, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (AnchorPresets.StretchAll):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
        }
    }

    public static void SetPivot(this RectTransform source, PivotPresets preset)
    {

        switch (preset)
        {
            case (PivotPresets.TopLeft):
                {
                    source.pivot = new Vector2(0, 1);
                    break;
                }
            case (PivotPresets.TopCenter):
                {
                    source.pivot = new Vector2(0.5f, 1);
                    break;
                }
            case (PivotPresets.TopRight):
                {
                    source.pivot = new Vector2(1, 1);
                    break;
                }

            case (PivotPresets.MiddleLeft):
                {
                    source.pivot = new Vector2(0, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleCenter):
                {
                    source.pivot = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (PivotPresets.MiddleRight):
                {
                    source.pivot = new Vector2(1, 0.5f);
                    break;
                }

            case (PivotPresets.BottomLeft):
                {
                    source.pivot = new Vector2(0, 0);
                    break;
                }
            case (PivotPresets.BottomCenter):
                {
                    source.pivot = new Vector2(0.5f, 0);
                    break;
                }
            case (PivotPresets.BottomRight):
                {
                    source.pivot = new Vector2(1, 0);
                    break;
                }
        }
    }
}


