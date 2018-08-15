using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Stopwatch = System.Diagnostics.Stopwatch;
using UnityEngine.UI;

public class MainPage : Page 
{
    public MainPage_Item sample_item;
    public Transform itemGridRoot;
    public Transform scrollContent;

    public RectTransform gift_button;

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

        Refrehs();

        TryGuid();
    }

    public void TryGuid()
    {
        var needGuide = PlayerStatus.needGide;
        if(needGuide)
        {
            UIEngine.ShowFloating<GuideFloating>(null, UIDepth.Top);
        }
    }

    private void Refrehs()
    {
        // create data list form static data
        var sheet = StaticDataLite.GetSheet("pictype");
        var dataList = new List<MainPage_ItemData>();
        foreach(string key in sheet.Keys)
        {
            var row = sheet[key];
            var data = new MainPage_ItemData
            {
                row = row,
                visible = true,
            };
            dataList.Add(data);
        }

        // 检查是否有未完成拼图
        {
            var count = PlayerStatus.uncompletePuzzle.Count;
            var data = new MainPage_ItemData()
            {
                pageType = PicturePageType.Uncomplete,
                visible = count > 0,
            };
            dataList.Insert(0, data);
        }

        // 检查是否有已完成的拼图
        {
            var count = PlayerStatus.completeDic.Count;
            var data = new MainPage_ItemData()
            {
                pageType = PicturePageType.Complete,
                visible = count > 0,
            };
            dataList.Insert(0, data);
            
        }

        SetDataList(dataList);
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
        RefreshOnlyCompleteAndUncomplete();
        RefreshAd();
    }

    public void RefreshOnlyCompleteAndUncomplete()
    {
        // 检查是否有未完成拼图
        {
            var count = PlayerStatus.uncompletePuzzle.Count;
            var data = new MainPage_ItemData()
            {
                pageType = PicturePageType.Uncomplete,
                visible = count > 0,
            };
            SetData(uncompleteItem, data);
        }

        // 检查是否有已完成的拼图
        {
            var count = PlayerStatus.completeDic.Count;
            var data = new MainPage_ItemData()
            {
                pageType = PicturePageType.Complete,
                visible = count > 0,
            };
            SetData(completeItem, data);
        }
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

    MainPage_Item completeItem;
    MainPage_Item uncompleteItem;

    MainPage_Item animal;

    void SetData(MainPage_Item item, MainPage_ItemData data)
    {
        // var sw = new Stopwatch();
        // sw.Start();

        item.data = data;
        item.gameObject.SetActive(data.visible);
        if(data.pageType == PicturePageType.Uncomplete)
        {
            uncompleteItem = item;
        }
        else if(data.pageType == PicturePageType.Complete)
        {
            completeItem = item;
        }
        if(!data.visible)
        {
            return;
        }

        // 如果是一个图片分类
        if(data.pageType == PicturePageType.Pictype)
        {
            item.label.text = data.row.Get<string>("display_name");
            var picType = data.row.Get<string>("id");
            var row = PicLibrary.FindFirstRowOfType(picType);
            var file = row?.Get<string>("file");
            var sprite = PicLibrary.LoadContentSprite(file);
            item.Facade = sprite;

            item.name = data.row.TryGet<string>("id", "no_id");
            if(item.name == "animal")
            {
                animal = item;
            }
        }

        // 如果是未完成的拼图
        if(data.pageType == PicturePageType.Uncomplete)
        {
            item.label.text = "Undown";
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
            item.label.text = "Done";
            CompleteInfo firstInfo = null;
            foreach(var kv in PlayerStatus.completeDic)
            {
                firstInfo = kv.Value;
                break;
            }
            var picId = firstInfo.pid;
            var picRow = StaticDataLite.GetRow("pic", picId.ToString());
            var fileName = picRow.Get<string>("file");
            var sprite = PicLibrary.LoadContentSprite(fileName);
            item.Facade = sprite;

        }

        
        // sw.Stop();
        // Debug.Log("set item: " + sw.Elapsed.TotalSeconds);
    }

    public void SimulateAnimeClick()
    {
        if(animal != null)
        {
            OnItemClick(animal);
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
        
        RadioStation.Brodcast("SELECT_PIC_TYPE");
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
        AdPage.sources = AdPageOpenSources.Shop;
        UIEngine.Forward<AdPage>(null, addmision);
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
    public void SetAdLabel(float time)
    {
        var t = (int)time;
        var label = gift_button.GetComponentInChildren<Text>();
        label.text = t.ToString();
    }

    public float adCountdown;
    public void RefreshAd()
    {
        Log.Scrren("MainPage: RefreshAd");
        var button = gift_button.GetComponentInChildren<Button>();
        var label = gift_button.GetComponentInChildren<Text>();
        adCountdown = PlayerStatus.CalcuNextAdSeconds();
        if(adCountdown > 0)
        {   
            button.interactable = false;
            SetAdLabel(adCountdown);
        }
        else
        {
            if(!SDKManager.IsAdLoaded)
            {
                button.interactable = false;
                label.text = "正在加载"; 
                waiteloading = true;
            }
            else
            {
                button.interactable = true;
                label.text = "可领取";
                waiteloading = false;
            }
        }
    }
}
