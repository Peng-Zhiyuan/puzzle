using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPage : Page 
{
    public MainPage_Item sample_item;
    public Transform itemGridRoot;

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

    void SetDataList(List<MainPage_ItemData> dataList)
    {
        // clean root
        for(int i = itemGridRoot.childCount - 1; i >= 0; i--)
        {
            var child = itemGridRoot.GetChild(i);
            Destroy(child);
        }

        // create items
        foreach(var data in dataList)
        {
            var item = GameObject.Instantiate(sample_item);
            item.transform.parent = itemGridRoot;
            item.gameObject.SetActive(true);
            item.Init(data);
        }
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
