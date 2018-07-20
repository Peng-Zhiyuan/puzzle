using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPage_Item : MonoBehaviour, IPointerClickHandler
{
    public MainPage_ItemData data;

    public void Init(MainPage_ItemData data)
    {
        this.data = data;
    }

	public void OnPointerClick(PointerEventData eventData)
	{
        SendMessageUpwards("OnItemClick", this);
	}

}