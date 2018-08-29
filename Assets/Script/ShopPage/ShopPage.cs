using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : Page
{
    public Transform transform_listRoot;
	
    public Transform prefab_ad_item;
    public Transform prefab_ipa_item;

    public RectTransform scrollViewContent;

    public List<ShopPage_Item> itemList;

    public override void OnCreate() 
    {
        prefab_ad_item.gameObject.SetActive(false);
        prefab_ipa_item.gameObject.SetActive(false);
        Refresh();
    }

    public override void OnNavigatedTo()
    {
        RefreshAd();
    }

    ShopPage_AdItem adItem;
    private void Refresh()
    {
        // rebuild item list
        itemList.Clear();
        TransformUtil.DestroyAllChildren(transform_listRoot);

        // add ad item first
        {
            var tr = GameObject.Instantiate(prefab_ad_item);
            tr.parent = transform_listRoot;
            tr.transform.localScale = Vector2.one;
            tr.gameObject.SetActive(true);
            var item = tr.GetComponent<ShopPage_Item>();
            itemList.Add(item);
            adItem = item as ShopPage_AdItem;
        }

        // add iap items
        var sheet = StaticDataLite.GetSheet("shop");
        foreach(string id in sheet.Keys)
        {
            var row = sheet[id];
            var tr = GameObject.Instantiate(prefab_ipa_item);
            tr.parent = transform_listRoot;
            tr.transform.localScale = Vector2.one;
            tr.gameObject.SetActive(true);
            var item = tr.GetComponent<ShopPage_IapItem>();
            item.Init(row);
            itemList.Add(item);
        }

        var itemHeight = prefab_ad_item.GetComponent<RectTransform>().rect.height;
        var gl = transform_listRoot.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
        var spaceingY = gl.spacing;
        var listInset = 487;
        var extra = 200;
        var rowCount = itemList.Count;
        var scrollContentHeight = itemHeight * rowCount + (rowCount - 1) * spaceingY + listInset + extra;
        var rt = scrollViewContent.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollContentHeight);
    }
    
    // 由 Item 的 sendMessage 调用
    public void ItemClicked(ShopPage_Item item)
    {
        AudioManager.PlaySe("button");
        if(item is ShopPage_IapItem)
        {
            var iapItem = item as ShopPage_IapItem;
            var row = iapItem.row;
            var id = row.Get<int>("id");
            SDKManager.Pay(id, buyed =>{
                if(buyed)
                {
                    Refresh();
                }
            });
        }
        else if(item is ShopPage_AdItem)
        {
            AdPage.sources = AdPageOpenSources.Shop;
            var popup = new Admission_PopupNewPage();
            UIEngine.Forward<AdPage>(null, popup);
        }
    }


    public void SetAdLabel(float time)
    {
        var t = (int)time;
        var label = adItem.transform.Find("label").GetComponent<Text>();
        label.text = t.ToString();
    }

    void Update()
    {
        if(adCountdown > 0)
        {
            adCountdown -= Time.deltaTime;
            SetAdLabel(adCountdown);
            if(adCountdown <= 0)
            {
                RefreshAd();
            }
        }
        else
        {
            if(waiteloading)
            {
                if(SDKManager.IsAdLoaded)
                {
                    RefreshAd();
                }
            }
        }
    }

    bool waiteloading;
    public float adCountdown;
    public void RefreshAd()
    {
        if(adItem != null)
        {
            var cg = adItem.GetComponentInChildren<CanvasGroup>();
            var label = adItem.transform.Find("label").GetComponent<Text>();
            adCountdown = PlayerStatus.CalcuNextAdSeconds();
            if(adCountdown > 0)
            {   
                cg.interactable = false;
                cg.blocksRaycasts = false;
                cg.alpha = 0.5f;
                SetAdLabel(adCountdown);
            }
            else
            {
                if(!SDKManager.IsAdLoaded)
                {
                    cg.interactable = false;
                    cg.blocksRaycasts = false;
                    cg.alpha = 0.5f;
                    label.text = "正在加载"; 
                    waiteloading = true;
                }
                else
                {
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                    cg.alpha = 1;
                    label.text = "";
                    waiteloading = false;
                }
            }
            //adItem.gameObject.SetActive(SDKManager.IsAdLoaded);
        }
    }

}
