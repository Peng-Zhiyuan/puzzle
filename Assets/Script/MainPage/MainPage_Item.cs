using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPage_Item : MonoBehaviour, IPointerClickHandler
{
    public MainPage_ItemData data;
    public Text label;
    public RawImage facade;


	public void OnPointerClick(PointerEventData eventData)
	{
        SendMessageUpwards("OnItemClick", this);
	}

    public Texture2D Facade
    {
        set
        {
            this.facade.texture = value;
        }
    }

}