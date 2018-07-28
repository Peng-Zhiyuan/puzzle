using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPage : Page
{
    public Transform transform_listRoot;
	
    public Transform prefab_ad_item;
    public Transform prefab_ipa_item;

    public List<ShopPage_Item> itemList;

    public override void OnCreate() 
    {
        prefab_ad_item.gameObject.SetActive(false);
        prefab_ipa_item.gameObject.SetActive(false);
    }

    public override void OnPush()
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
        
    }
    
    // 由 Item 的 sendMessage 调用
    public void ItemClicked(ShopPage_Item item)
    {
        if(item is ShopPage_IapItem)
        {
            var iapItem = item as ShopPage_IapItem;
            var row = iapItem.row;
            Debug.Log(row.Get<int>("id"));
        }
        else if(item is ShopPage_AdItem)
        {
            var popup = new Admission_PopupNewPage();
            UIEngine.Forward<AdPage>(null, popup);
        }
    }

}
