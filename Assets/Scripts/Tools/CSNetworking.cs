using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CSNetworking
{

    #region Post

    public static void PostAsync(string url, string content, Action<object[]> succesCallBack, RequestType requestType = RequestType.String, Dictionary<string, string> keyValuePairs = null)
    {
        HttpClientPostAsync(url, content, (data) =>
         {
             try
             {
                 Loom.QueueOnMainThread(() =>
                 {
                     succesCallBack?.Invoke(data);
                 });
             }
             catch (Exception e)
             {
                 Debug.LogError(e.StackTrace);
             }

         }, requestType, keyValuePairs);
    }

    public static void HttpClientPostAsync(string url, string content, Action<object[]> action, RequestType requestType, Dictionary<string, string> keyValuePairs)
    {
        new Task(() =>
        {
            HttpClientPostInvoke(url, content, action, requestType, keyValuePairs);
        }).Start();
    }

    private static async void HttpClientPostInvoke(string url, string content, Action<object[]> action, RequestType requestType, Dictionary<string, string> keyValuePairs)
    {
        //等待返回
        object[] obj = await HttpClientPost(url, content, requestType, keyValuePairs);
        //输出返回
        if (obj != null)
        {
            action?.Invoke(obj);
        }
    }

    private static async Task<object[]> HttpClientPost(string url, string _content, RequestType requestType, Dictionary<string, string> keyValuePairs)
    {
        HttpClient httpClient = new HttpClient();
        //httpClient.DefaultRequestHeaders.Add("User-Agent", "Hangyan_ShangraoProject 1.0.0 PC");
        if (keyValuePairs != null)
        {
            foreach (var item in keyValuePairs)
            {
                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }
        HttpContent httpContent = new StringContent(_content, Encoding.UTF8, "application/json");
        object[] contentObj = new object[1];

        try
        {
            var response = await httpClient.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            switch (requestType)
            {
                case RequestType.ByteArray:
                    contentObj[0] = await response.Content.ReadAsByteArrayAsync();
                    break;
                case RequestType.Stream:
                    contentObj[0] = await response.Content.ReadAsStreamAsync();
                    break;
                case RequestType.String:
                    contentObj[0] = await response.Content.ReadAsStringAsync();
                    break;
            }
            //httpContent.Dispose();
        }
        catch (Exception e)
        {
            Debug.LogError("=============------" + e);
            //httpContent.Dispose();
        }

        //httpContent.Dispose();
        return contentObj;
    }


    #endregion

    #region Get

    public static void GetAsync(string url, Action<object[]> succesCallBack, RequestType requestType = RequestType.String, Dictionary<string, string> keyValuePairs = null)
    {
        HttpClientGetAsync(url, (data) =>
        {
            try
            {
                Loom.QueueOnMainThread(() =>
                {
                    succesCallBack?.Invoke(data);
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"[FXNetworking] Get报错 {e.StackTrace}");
            }

        }, requestType, keyValuePairs);
    }

    private static void HttpClientGetAsync(string url, Action<object[]> action, RequestType requestType, Dictionary<string, string> keyValuePairs)
    {
        new Task(() =>
        {
            HttpClientGetInvoke(url, action, requestType, keyValuePairs);
        }).Start();
    }

    private static async void HttpClientGetInvoke(string url, Action<object[]> action, RequestType requestType, Dictionary<string, string> keyValuePairs)
    {
        //等待返回
        object[] obj = await HttpClientGet(url, requestType, keyValuePairs);
        //输出返回
        if (obj != null)
        {
            action?.Invoke(obj);
        }
    }

    private static async Task<object[]> HttpClientGet(string url, RequestType requestType, Dictionary<string, string> keyValuePairs)
    {
        var uri = new Uri(url);
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Hangyan_ShangraoProject 1.0.0 PC");
        if (keyValuePairs != null)
        {
            foreach (var item in keyValuePairs)
            {
                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }
        object[] contentObj = new object[1];
        switch (requestType)
        {
            case RequestType.ByteArray:
                contentObj[0] = await httpClient.GetByteArrayAsync(uri);
                break;
            case RequestType.Stream:
                contentObj[0] = await httpClient.GetStreamAsync(uri);
                break;
            case RequestType.String:
                contentObj[0] = await httpClient.GetStringAsync(uri);
                break;
        }
        //Debug.Log($"URL>>>{url}");
        return contentObj;
    }

    #endregion


}

public enum RequestType
{
    ByteArray,
    Stream,
    String,

}
