using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomLitJson;
using UnityEngine.UI;

public class ShopPage_IapItem : ShopPage_Item 
{
    public Transform tag1;
    public Transform tag2;
    public Transform tag3;

    public Transform icon1;
    public Transform icon2;
    public Transform icon3;
    public Transform iconAd;
    public Text text_gold;
    public Text text_price;

    public JsonData row;

    public void Init(JsonData row)
    {
        this.row = row;
        var id = row.TryGet("id", -1);
        var type = row.TryGet("type", 1);
        var gold = row.TryGet("gold", 0);
        var price = row.TryGet("price", 0f);
        var func = row.TryGet("func", "");
        var funcDes = row.TryGet("func_des", "");

        this.Type = type;
        if(string.IsNullOrEmpty(func))
        {
            this.text_gold.text = gold.ToString();
        }
        else
        {
            this.text_gold.text = funcDes;
        }
        this.text_price.text = "¥ " + price.ToString("F2");

        if(func == "REMOVE_AD")
        {
            if(PlayerStatus.removeAd)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    int Type
    {
        set
        {
            TagType = value;
            IconType = value;
        }
    }

    int TagType
    {
        set
        {
            if(value == 1)
            {
                tag1.gameObject.SetActive(true);
                tag2.gameObject.SetActive(false);
                tag3.gameObject.SetActive(false);
            }
            else if(value == 2)
            {
                tag1.gameObject.SetActive(false);
                tag2.gameObject.SetActive(true);
                tag3.gameObject.SetActive(false);
            }
            else if(value >= 3)
            {
                tag1.gameObject.SetActive(false);
                tag2.gameObject.SetActive(false);
                tag3.gameObject.SetActive(true);
            }
            else if(value == -1)
            {
                tag1.gameObject.SetActive(false);
                tag2.gameObject.SetActive(false);
                tag3.gameObject.SetActive(false);
            }
        }  
    }

    int IconType
    {
        set
        {
            if(value == 1)
            {
                icon1.gameObject.SetActive(true);
                icon2.gameObject.SetActive(false);
                icon3.gameObject.SetActive(false);
                iconAd.gameObject.SetActive(false);
            }
            else if(value == 2)
            {
                icon1.gameObject.SetActive(false);
                icon2.gameObject.SetActive(true);
                icon3.gameObject.SetActive(false);
                iconAd.gameObject.SetActive(false);
            }
            else if(value >= 3)
            {
                icon1.gameObject.SetActive(false);
                icon2.gameObject.SetActive(false);
                icon3.gameObject.SetActive(true);
                iconAd.gameObject.SetActive(false);
            }
            else if(value == -1)
            {
                icon1.gameObject.SetActive(false);
                icon2.gameObject.SetActive(false);
                icon3.gameObject.SetActive(false);
                iconAd.gameObject.SetActive(true);
            }
        }  
    }

    string Line1
    {
        set
        {
            tag1.Find("1").GetComponent<Text>().text = value;
            tag2.Find("1").GetComponent<Text>().text = value;
            tag3.Find("1").GetComponent<Text>().text = value;
        }
    }

    string Line2
    {
        set
        {
            tag1.Find("2").GetComponent<Text>().text = value;
            tag2.Find("2").GetComponent<Text>().text = value;
            tag3.Find("2").GetComponent<Text>().text = value;
        }
    }
}
