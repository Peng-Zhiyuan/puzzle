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
        // ini samplify-scrollView
        SetDataList(dataList);
    }

    public override void OnNavigatedTo()
    {
        UIEngine.ShowFlaoting("BackgroundFloating");
    
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
        item.label.text = data.row.Get<string>("display_name");
        var file = PicLibrary.FindFirstFileNameOfType(data.row.Get<string>("id"));
        var texture = PicLibrary.Load(file);
        item.Facade = texture;
    }

    public void OnItemClick(MainPage_Item item)
    {
        var pictype = item.data.row.Get<string>("id");
        var rt = item.GetComponent<RectTransform>();
		var rect = GetWorldRect(rt, Vector2.one);
	
		var admission = new Admission_ScaleUpNewPage(rect);
		UIEngine.Forward<PicturePage>(pictype, admission);

        HeadBarFloating.admission = new Admission_ScaleDownOldPage(rect);
    }


    static public Rect GetWorldRect (RectTransform rt, Vector2 scale) {
         // Convert the rectangle to world corners and grab the top left
         Vector3[] corners = new Vector3[4];
         rt.GetWorldCorners(corners);
         Vector3 topLeft = corners[0];
 
         // Rescale the size appropriately based on the current Canvas scale
         Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);
 
         return new Rect(topLeft, scaledSize);
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
