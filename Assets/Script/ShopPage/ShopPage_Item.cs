using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopPage_Item : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        SendMessageUpwards("ItemClicked", this, SendMessageOptions.RequireReceiver);
    }
}
