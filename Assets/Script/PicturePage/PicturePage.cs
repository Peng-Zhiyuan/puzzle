using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomLitJson;

public class PicturePage : Page
{
    public Text text_title;
    public Text text_des;
    public PictruePage_Item itemSample;
    public Transform itemRoot;
    public RectTransform scrollContent;
    public Text text_pictureCount;


    public override void OnParamChanged()
    {
        var pictype = this.param;
        var name = StaticDataLite.GetCell<string>("pictype", pictype, "display_name");
        var des = StaticDataLite.GetCell<string>("pictype", pictype, "des");
        this.text_title.text = name;
        this.text_des.text = des;
    }

    public override void OnPush()
    {
        // 从 pic 表中找出所有 type 是 param 的数据行
        // 并且包装成 PicturePage_ItemData
        var sheet = StaticDataLite.GetSheet("pic");
        var pictype = this.param;
        var dataList = new List<PictruePage_ItemData>();
        foreach(string key in sheet.Keys)
        {
            var row = sheet[key];
            if(row.Get<string>("type") == pictype)
            {
                var data = new PictruePage_ItemData
                {
                    row = row,
                };
                // check unlock info
                var unlocked = LevelStorage.IsPictureUnlocked(key);
                var complete = LevelStorage.IsPictureComplete(key);
                var status = PicturePage_ItemStatus.Locked;
                if(complete)
                {
                    status = PicturePage_ItemStatus.Complete;
                }
                else if(unlocked)
                {
                    status = PicturePage_ItemStatus.Unlocked;
                }
                data.status = status;
                dataList.Add(data);
            }
        }
        this.setDataList(dataList);

        // set picture count
        var completePictureCount = 0;
        var pictureCount = dataList.Count;
        foreach(var data in dataList)
        {
            if(data.status == PicturePage_ItemStatus.Complete)
            {
                completePictureCount++;
            }
        }
        text_pictureCount.text = completePictureCount + "/" + pictureCount;
    }

    void setDataList(List<PictruePage_ItemData> dataList)
    {
        // clean old items 
        TransformUtil.DestroyAllChildren(itemRoot);

        // create new items for each data
        foreach(var data in dataList)
        {
            var item = GameObject.Instantiate(itemSample);
            item.transform.parent = itemRoot;
            SetItem(item, data);
        }

        // set scroll content total height
        var itemHeight = itemSample.GetComponent<RectTransform>().rect.height;
        var spaceing = itemRoot.GetComponent<VerticalLayoutGroup>().spacing;
        var listInset = 480;
        var scrollContentHeight = itemHeight * dataList.Count + (dataList.Count - 1) * spaceing + listInset;
        var rt = scrollContent.GetComponent<RectTransform>();
        Debug.Log(scrollContentHeight);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollContentHeight);

    }

    void SetItem(PictruePage_Item item, PictruePage_ItemData data)
    {
        item.data = data;
        switch(data.status)
        {
            case PicturePage_ItemStatus.Locked:
                item.IsShowPuzzleMask = true;
                item.IsShowUnlockLayer = true;
                break;
            case PicturePage_ItemStatus.Unlocked:
                item.IsShowPuzzleMask = true;
                item.IsShowUnlockLayer = false;
                break;
            case PicturePage_ItemStatus.Complete:
                item.IsShowPuzzleMask = false;
                item.IsShowUnlockLayer = false;
                break;
        }
    }

    void OnItemUnlockButton(PictruePage_Item item)
    {
        var data = item.data;
        var cost = data.row.Get<int>("cost");
        var gold = GameStorage.Gold;
        if(gold >= cost)
        {
            Debug.Log("can unlock");
            gold -= cost;
            GameStorage.Gold = gold;
            var pictureId = data.row.Get<string>("id");
            LevelStorage.SetPictureUnlocked(pictureId);
            PlayerPrefs.Save();
            // 单独处理需要修改显示状态的 item
            data.status = PicturePage_ItemStatus.Unlocked;
            SetItem(item, data);
        }
        else
        {
            var text = MsgList.Get("lacke_of_gold");
            UIEngine.Forward<DialogPage>(text);
        }
        
    }

    void OnItemClicked(PictruePage_Item item)
    {
        if(item.data.status == PicturePage_ItemStatus.Unlocked)
        {
            //UIEngine.Forward<LevelCompletePage>();
            var picId = item.data.row.Get<int>("id");
            // GameController.EnterCore(picId);
            var admin = new Admission_PopupNewPage();
            UIEngine.Forward<LevelSettingsPage>(picId.ToString(), admin);
        }
    }
}
