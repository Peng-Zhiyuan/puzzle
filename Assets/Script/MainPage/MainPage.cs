using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var count = PlayerStatus.uncompletePuzzle.Count;
        if(count > 0)
        {
            var data = new MainPage_ItemData()
            {
                isUncompletePuzzle = true
            };
            dataList.Insert(0, data);
        }

        SetDataList(dataList);
    }

    void SetDataList(List<MainPage_ItemData> dataList)
    {
        // clean root
        TransformUtil.DestroyAllChildren(itemGridRoot);

        // create items
        foreach(var data in dataList)
        {
            var item = GameObject.Instantiate(sample_item);
            item.transform.parent = itemGridRoot;
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
        Debug.Log(scrollContentHeight);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollContentHeight);
    }

    void SetData(MainPage_Item item, MainPage_ItemData data)
    {
        item.data = data;

        // 如果是一个图片分类
        if(!data.isUncompletePuzzle)
        {
            item.label.text = data.row.Get<string>("display_name");
            var file = PicLibrary.FindFirstFileNameOfType(data.row.Get<string>("id"));
            var texture = PicLibrary.Load(file);
            item.Facade = texture;
        }

        // 如果是未完成的拼图
        if(data.isUncompletePuzzle)
        {
            item.label.text = "未完成的拼图";
            if(PlayerStatus.uncompletePuzzle.Count > 0)
            {
                var firstCoreInfo = PlayerStatus.FirstUncompletePuzzleInfo;
                var picId = firstCoreInfo.picId;
                var picRow = StaticDataLite.GetRow("pic", picId.ToString());
                var fileName = picRow.Get<string>("file");
                var texture = PicLibrary.Load(fileName);
                item.Facade = texture;
            }
        }
    }

    public void OnItemClick(MainPage_Item item)
    {
        string param = "#uncomplete";
        if(item.data.isUncompletePuzzle)
        {
            param = "#uncomplete";
        }
        else
        {
            var pictype = item.data.row.Get<string>("id");
            param = pictype;
        }
        var rt = item.GetComponent<RectTransform>();
		var rect = RectTransformUtil.GetWorldRect(rt);
	
		var admission = new Admission_ScaleUpNewPage(rect);
		UIEngine.Forward<PicturePage>(param, admission);

        HeadBarFloating.admission = new Admission_ScaleDownOldPage(rect);
    }

    public void OnShopButton()
    {
        var admission = new Admission_OldDownNewUp();
        UIEngine.Forward<ShopPage>(null, admission);
        HeadBarFloating.admission = new Admission_OldDownNewUp();
    }
	
    public void OnGiftButton()
    {
        var addmision = new Admission_PopupNewPage();
        UIEngine.Forward<AdPage>(null, addmision);
    }
}
