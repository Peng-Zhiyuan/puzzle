﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainPage : Page 
{
    public MainPage_Item sample_item;
    public Transform itemGridRoot;
    public Transform scrollContent;

    public override void OnCreate()
    {
        sample_item.gameObject.SetActive(false);
        // create data list form static data
        // var sheet = StaticDataLite.GetSheet("pictype");
        // var dataList = new List<MainPage_ItemData>();
        // foreach(string key in sheet.Keys)
        // {
        //     var row = sheet[key];
        //     var data = new MainPage_ItemData
        //     {
        //         row = row,
        //     };
        //     dataList.Add(data);
        // }

        // // 检查是否有未完成拼图
        // var count = PlayerStatus.uncompletePuzzle.Count;
        // if(count > 0)
        // {
        //     var data = new MainPage_ItemData()
        //     {
        //         isUncompletePuzzle = true
        //     };
        //     dataList.Insert(0, data);
        // }

        // // ini samplify-scrollView
        // SetDataList(dataList);
    }

    public override void OnPush()
    {
        var floating = UIEngine.ShowFloating<BackgroundFloating>(null, UIDepth.Low);
		floating.transform.SetAsFirstSibling();
		UIEngine.ShowFloating<HeadBarFloating>();

        if(!PlayerStatus.IsTodaySigned())
        {
            CoroutineManager.Create(WaitAndShowSign());
        }
    }

    public IEnumerator WaitAndShowSign()
    {
        yield return new WaitForSeconds(0.5f);
        var admin = new Admission_PopupNewPage();
        UIEngine.Forward<SignPage>(null, admin);
    }

    public override void OnNavigatedTo()
    {
        UIEngine.ShowFlaoting("BackgroundFloating");
    
        // create data list form static data
        var sheet = StaticDataLite.GetSheet("pictype");
        var dataList = new List<MainPage_ItemData>();
        foreach(string key in sheet.Keys)
        {
            var row = sheet[key];
            var data = new MainPage_ItemData
            {
                row = row,
            };
            dataList.Add(data);
        }

        // 检查是否有未完成拼图
        {
            var count = PlayerStatus.uncompletePuzzle.Count;
            if(count > 0)
            {
                var data = new MainPage_ItemData()
                {
                    pageType = PicturePageType.Uncomplete
                };
                dataList.Insert(0, data);
            }
        }

        // 检查是否有已完成的拼图
        {
            var count = PlayerStatus.completeDic.Count;
            if(count > 0)
            {
                var data = new MainPage_ItemData()
                {
                    pageType = PicturePageType.Complete
                };
                dataList.Insert(0, data);
            }
        }
        
        SetDataList(dataList);
    }

    Queue<MainPage_Item> pool = new Queue<MainPage_Item>();
    void PutItem(MainPage_Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = null;
        pool.Enqueue(item);
    }

    MainPage_Item TakeItem()
    {
        if(pool.Count > 0)
        {
            return pool.Dequeue();
        }
        var item = GameObject.Instantiate(sample_item);
        return item;
    }    

    void SetDataList(List<MainPage_ItemData> dataList)
    {
        // clean root
        //TransformUtil.DestroyAllChildren(itemGridRoot);
		for(int i = itemGridRoot.childCount - 1; i >= 0; i--)
		{
			var child = itemGridRoot.GetChild(i).GetComponent<MainPage_Item>();
            PutItem(child);
		}

        // create items
        foreach(var data in dataList)
        {
            //var item = GameObject.Instantiate(sample_item);
            var item = TakeItem();
            item.transform.parent = itemGridRoot;
            item.transform.localScale = Vector2.one;
            item.gameObject.SetActive(true);
            //item.Init(data);
            SetData(item, data);
        }

        // reset scroll content height
        var itemHeight = sample_item.GetComponent<RectTransform>().rect.height;
        var gl = itemGridRoot.GetComponent<UnityEngine.UI.GridLayoutGroup>();
        var spaceingY = gl.spacing.y;
        var listInset = 436;
        var extra = 200;
        var rowCount = Mathf.Ceil(dataList.Count/2f);
        var scrollContentHeight = itemHeight * rowCount + (rowCount - 1) * spaceingY + listInset + extra;
        var rt = scrollContent.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollContentHeight);
    }

    void SetData(MainPage_Item item, MainPage_ItemData data)
    {
        item.data = data;

        // 如果是一个图片分类
        if(data.pageType == PicturePageType.Pictype)
        {
            item.label.text = data.row.Get<string>("display_name");
            var picType = data.row.Get<string>("id");
            var row = PicLibrary.FindFirstRowOfType(picType);
            var file = row?.Get<string>("file");
            var sprite = PicLibrary.LoadContentSprite(file);
            item.Facade = sprite;
        }

        // 如果是未完成的拼图
        if(data.pageType == PicturePageType.Uncomplete)
        {
            item.label.text = "未完成";
            if(PlayerStatus.uncompletePuzzle.Count > 0)
            {
                var firstCoreInfo = PlayerStatus.FirstUncompletePuzzleInfo;
                var picId = firstCoreInfo.picId;
                var picRow = StaticDataLite.GetRow("pic", picId.ToString());
                var fileName = picRow.Get<string>("file");
                var sprite = PicLibrary.LoadContentSprite(fileName);
                item.Facade = sprite;
            }
        }

        // 如果是已完成的拼图
        if(data.pageType == PicturePageType.Complete)
        {
            item.label.text = "已完成";
            foreach(var kv in PlayerStatus.completeDic)
            {
                var record = kv.Value;
                var picId = record.pid;
                var picRow = StaticDataLite.GetRow("pic", picId.ToString());
                var fileName = picRow.Get<string>("file");
                var sprite = PicLibrary.LoadContentSprite(fileName);
                item.Facade = sprite;
            }
        }
    }

    public void OnItemClick(MainPage_Item item)
    {
        AudioManager.PlaySe("button");

        var param = new PicturePageParam();
        param.pageType = item.data.pageType;
        if(item.data.pageType == PicturePageType.Pictype)
        {
            param.picTypeId = item.data.row.Get<string>("id");
        }

        var rt = item.GetComponent<RectTransform>();
		var rect = RectTransformUtil.GetWorldRect(rt);
	
		var admission = new Admission_ScaleUpNewPage(rect);
		UIEngine.Forward<PicturePage>(param, admission);

        HeadBarFloating.admission = new Admission_ScaleDownOldPage(rect);
    }

    public void OnShopButton()
    {
        AudioManager.PlaySe("sign-and-shop");
        var admission = new Admission_OldDownNewUp();
        UIEngine.Forward<ShopPage>(null, admission);
        HeadBarFloating.admission = new Admission_OldDownNewUp();
    }
	
    public void OnGiftButton()
    {
        AudioManager.PlaySe("sign-and-shop");
        var addmision = new Admission_PopupNewPage();
        UIEngine.Forward<AdPage>(null, addmision);
    }
}
