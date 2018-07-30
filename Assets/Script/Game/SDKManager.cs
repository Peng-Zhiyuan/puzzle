using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SDKManager
{
    public static void OnHeadBarBackbutton()
    {
        var val = Random.Range(1, 11);
        if(val <= 3)
        {
            ShowFullAd();
        }
    }

    public static void ShowFullAd()
    {
        Debug.Log("[SDKManager] Show full ad");
    }

    public static void Comment()
    {
        Debug.Log("[SDKManager] goto comment");
    }
}