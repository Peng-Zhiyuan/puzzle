using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomLitJson;
using System;

public static class SDKManager
{
    public static void OnHeadBarBackbutton()
    {
        var val = UnityEngine.Random.Range(1, 11);
        if(val <= 3)
        {
            ShowFullAd();
        }
    }

    public static bool IsAdLoaded
    {
        get
        {
            return IsInterAdLoaded();
        }
    }

    public static void ShowFullAd()
    {
        Debug.Log("[SDKManager] Show full ad");
    }

    public static void Comment()
    {
        Debug.Log("[SDKManager] goto comment");
        NativeBridge.InvokeCall("NativeSDKManager", "GotoMarket", null, result =>{
            Helper.AddGold(40);
        });
    }

    public static void Exit()
    {
        NativeBridge.SendNotify("NativeSDKManager", "Exit");
    } 

    public static void Pay(int goodsId, Action<bool> onComplete = null)
    {
        var row = StaticDataLite.GetRow("shop", goodsId.ToString());
        var productName = row.Get<string>("product_name");
        var productDesc = row.Get<string>("product_desc");
        var price_yuan = row.Get<float>("price");
        var gold = row.Get<int>("gold");
        var func = row.Get<string>("func");
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

        NativeBridge.InvokeCall("NativeSDKManager", "Pay", json, result => 
        {
            if(result == "SUCCESS")
            {
                Log.Scrren("SUCCESS");
                Helper.AddGold(gold);
                if(!string.IsNullOrEmpty(func))
                {
                    if(func == "REMOVE_AD")
                    {
                        PlayerStatus.removeAd = true;
                    }
                }
                PlayerStatus.Save();
                if(onComplete != null)
                {
                    onComplete(true);
                }
            }
            else if(result == "FAIL")
            {
                Log.Scrren("FAIL");
                if(onComplete != null)
                {
                    onComplete(false);
                }
            }
            else if(result == "CANCEL")
            {
                Log.Scrren("CANCEL");
                if(onComplete != null)
                {
                    onComplete(false);
                }
            }
        });
    }

    public static void OnEnterCore()
    {
        if(PlayerStatus.removeAd)
        {
            return;
        }
        NativeBridge.SendNotify("NativeSDKManager", "OnEnterCore");
    }

    public static void OnExitCore()
    {
        NativeBridge.SendNotify("NativeSDKManager", "OnExitCore");
    }

    public static bool IsInterAdLoaded()
    {
        if(PlayerStatus.removeAd)
        {
            return true;
        }
        var ret = NativeBridge.SyncCall("NativeSDKManager", "IsInterAdLoaded");
        if(ret == "true")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ShowInterAd(Action<bool> callback)
    {
        if(PlayerStatus.removeAd)
        {
            callback(true);
            return;
        }
        NativeBridge.InvokeCall("NativeSDKManager", "ShowInterAd", null, result=>{
            if(result == "true")
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        });
    }

}