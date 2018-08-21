using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomLitJson;

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

    public static bool IsAdLoaded
    {
        get
        {
            return true;
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

    public static void Exit()
    {
        NativeBridge.SendNotify("NativeSDKManager", "Exit");
    } 

    public static void Pay(int goodsId)
    {
        var row = StaticDataLite.GetRow("shop", goodsId.ToString());
        var productName = row.Get<string>("product_name");
        var productDesc = row.Get<string>("product_desc");
        var price_yuan = row.Get<float>("price");
        var gold = row.Get<int>("gold");
        var itemId = goodsId;
        var price_fen = price_yuan * 100;

        var jd = new JsonData();
        jd["productName"] = productName;
        jd["productDesc"] = productDesc;
        jd["price_fen"] = price_fen;
        jd["price_yuan"] = price_yuan;
        jd["itemId"] = itemId;
        jd["gold"] = gold;
        var json = jd.ToJson();

        Helper.AddGold(gold);

        // NativeBridge.InvokeCall("NativeSDKManager", "Pay", json, result => 
        // {
        //     if(result == "SUCCESS")
        //     {
        //         Log.Scrren("SUCCESS");
        //         Helper.AddGold(gold);
        //     }
        //     else if(result == "FAIL")
        //     {
        //         Log.Scrren("FAIL");
        //     }
        //     else if(result == "CANCEL")
        //     {
        //         Log.Scrren("CANCEL");
        //     }
        // });
    }
}