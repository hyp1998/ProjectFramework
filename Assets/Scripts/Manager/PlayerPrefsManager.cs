using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager
{
    public static void SetToken(string code = "")
    {
        PlayerPrefs.SetString(ProjectConst.PlayerPrefsToken, code);
    }

    public static string GetToken()
    {
        return PlayerPrefs.GetString(ProjectConst.PlayerPrefsToken);
    }

    public static bool GetTokenState()
    {
        return string.IsNullOrEmpty(PlayerPrefs.GetString(ProjectConst.PlayerPrefsToken));
    }
}
