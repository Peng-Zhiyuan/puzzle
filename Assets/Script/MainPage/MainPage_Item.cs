using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPage_Item : MonoBehaviour, IPointerClickHandler
{
    public MainPage_ItemData data;
    public Text label;
    public Image facade;


	public void OnPointerClick(PointerEventData eventData)
	{
        if(data != null)
        {
            var top = UIEngine.Top;
            if(top.name == "MainPage")
            {
                var mainPage = top as MainPage;
                mainPage.OnItemClick(this);
            }
        }
        else
        {
            // this is guide mode
            var top = UIEngine.Top;
            if(top.name == "MainPage")
            {
                var mainPage = top as MainPage;
                mainPage.SimulateAnimeClick();
            }
        }

	}

    public Sprite Facade
    {
        set
        {
            this.facade.sprite = value;
        }
    }

}