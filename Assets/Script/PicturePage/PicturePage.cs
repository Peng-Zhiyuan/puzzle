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

    public PicturePageParam p;

    public override void OnParamChanged()
    {
        this.p = param as PicturePageParam;
        if(p.pageType == PicturePageType.Uncomplete)
        {
            this.text_title.text = "未完成";
            this.text_des.text = "共 " + PlayerStatus.uncompletePuzzle.Count + " 张";
        }
        else if(p.pageType == PicturePageType.Pictype)
        {
            var pictype = p.picTypeId;
            var name = StaticDataLite.GetCell<string>("pictype", pictype, "display_name");
            var des = StaticDataLite.GetCell<string>("pictype", pictype, "des");
            this.text_title.text = name;
            this.text_des.text = des;
        }
        else if(p.pageType == PicturePageType.Complete)
        {
            this.text_title.text = "已完成";
            this.text_des.text = "共 " + PlayerStatus.completeDic.Count + " 张";
        }

    }

    public override void OnPush()
    {
        // 如果要显示的是某个图片分类
        if(p.pageType == PicturePageType.Pictype)
        {
            // 从 pic 表中找出所有 type 是 param 的数据行
            // 并且包装成 PicturePage_ItemData
            var sheet = StaticDataLite.GetSheet("pic");
            var pictype = this.p.picTypeId;
            var dataList = new List<PictruePage_ItemData>();
            foreach(string key in sheet.Keys)
            {
                var row = sheet[key];
                if(row.Get<string>("type") == pictype)
                {
                    var data = new PictruePage_ItemData
                    {
                        picRow = row,
                    };
                    // check unlock info
                    var unlocked = LevelStorage.IsPictureUnlocked(key);
                    var complete = PlayerStatus.IsPictureComplete(int.Parse(key));
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

        // 如果要显示的是未完成的拼图
        if(p.pageType == PicturePageType.Uncomplete)
        {
            var dataList = new List<PictruePage_ItemData>();
            foreach(var kv in PlayerStatus.uncompletePuzzle)
            {
                var info = kv.Value;
                var picRow = StaticDataLite.GetRow("pic", info.picId.ToString());

                var data = new PictruePage_ItemData
                {
                    picRow = picRow,
                };
                data.status = PicturePage_ItemStatus.Unlocked;
                dataList.Add(data);
            }
            this.setDataList(dataList);

            text_pictureCount.text = PlayerStatus.uncompletePuzzle.Count.ToString();

        }

        // 如果要显示已完成的图片
        if(p.pageType == PicturePageType.Complete)
        {
            var dataList = new List<PictruePage_ItemData>();
            foreach(var kv in PlayerStatus.completeDic)
            {
                var info = kv.Value;
                var picId = info.pid;
                var picRow = StaticDataLite.GetRow("pic", picId.ToString());

                var data = new PictruePage_ItemData
                {
                    picRow = picRow,
                };
                data.status = PicturePage_ItemStatus.Complete;
                dataList.Add(data);
            }
            text_pictureCount.text = dataList.Count.ToString();
            this.setDataList(dataList);
        }
      
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
            item.transform.localScale = Vector2.one;
            SetItem(item, data);
            item.gameObject.SetActive(true);
        }

        // set scroll content total height
        var itemHeight = itemSample.GetComponent<RectTransform>().rect.height;
        var spaceing = itemRoot.GetComponent<VerticalLayoutGroup>().spacing;
        var preExtra = 480;
        var postExtra = 100;
        var scrollContentHeight = itemHeight * dataList.Count + (dataList.Count - 1) * spaceing + preExtra + postExtra;
        var rt = scrollContent.GetComponent<RectTransform>();
        Debug.Log(scrollContentHeight);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollContentHeight);

        // hide sample
        itemSample.gameObject.SetActive(false);

    }

    void SetItem(PictruePage_Item item, PictruePage_ItemData data)
    {
        item.data = data;
        switch(data.status)
        {
            case PicturePage_ItemStatus.Locked:
                item.IsShowPuzzleMask = true;
                item.IsShowUnlockLayer = true;
                item.IsShowPice = false;
                break;
            case PicturePage_ItemStatus.Unlocked:
                item.IsShowPuzzleMask = true;
                item.IsShowUnlockLayer = false;
                item.IsShowPice = false;
                break;
            case PicturePage_ItemStatus.Complete:
                item.IsShowPuzzleMask = false;
                item.IsShowUnlockLayer = false;
                item.IsShowPice = true;
                var picId = data.picRow.Get<int>("id");
                var info = PlayerStatus.GetCompleteInfoOfPicId(picId);
                if(info != null)
                {
                    var sliceCount = StaticDataLite.GetCell<int>("pice_slice", info.sliceId.ToString(), "count");
                    item.PiceCount = sliceCount;
                }
                break;
        }
        item.LabelText = data.picRow.Get<string>("name");
        var file = data.picRow.Get<string>("file");
        var texture = PicLibrary.Load(file);
        item.Texture2D = texture;
    }

    void OnItemUnlockButton(PictruePage_Item item)
    {
        var data = item.data;
        var cost = data.picRow.Get<int>("cost");
        var gold = PlayerStatus.gold;
        if(gold >= cost)
        {
            Debug.Log("can unlock");
            gold -= cost;
            PlayerStatus.gold = gold;
            var pictureId = data.picRow.Get<string>("id");
            LevelStorage.SetPictureUnlocked(pictureId);
            PlayerStatus.Save();
            
            // 单独处理需要修改显示状态的 item
            data.status = PicturePage_ItemStatus.Unlocked;
            SetItem(item, data);
            item.Flash();
        }
        else
        {
            var text = MsgList.Get("lack_of_gold");
            var popup = new Admission_PopupNewPage();
            UIEngine.Forward<DialogPage>(text, popup);
        }
        
    }

    void OnItemClicked(PictruePage_Item item)
    {
        // 如果是图片分类，则开始新游戏
        //if(!isUncomplete)
        //{
            if(item.data.status != PicturePage_ItemStatus.Locked)
            {
                //UIEngine.Forward<LevelCompletePage>();
                var picId = item.data.picRow.Get<int>("id");
                // GameController.EnterCore(picId);
                var admin = new Admission_PopupNewPage();
                UIEngine.Forward<LevelSettingsPage>(picId.ToString(), admin);
            }
        //}

        // 如果是未完成的拼图, 则继续游戏
        // if(isUncomplete)
        // {
        //     var picId = item.data.picRow.Get<int>("id");
        //     var info = PlayerStatus.uncompletePuzzle[picId.ToString()];
        //     GameController.EnterWithInfo(info);
        //     // GameController.EnterCore(picId);
        //     // var admin = new Admission_PopupNewPage();
        //     // UIEngine.Forward<LevelSettingsPage>(picId.ToString(), admin);
        // }

    }
}
