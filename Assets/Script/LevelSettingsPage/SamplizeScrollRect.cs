using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class SamplizeScrollRect : MonoBehaviour, IEndDragHandler
{
	public RectTransform sample;
	public LayoutGroup layout;

	private RectTransform _content;
	public RectTransform Content
	{
		get
		{
			if(_content == null)
			{
				_content = this.GetComponent<ScrollRect>().content;
			}
			return _content;
		}
	}

	private List<Transform> itemList = new List<Transform>();

	public Action<Transform, object> OnSetData;

	public void ChangeData(IList<object> dataList)
	{
		TransformUtil.DestroyAllChildren(layout.transform);
		itemList.Clear();
		foreach(var data in dataList)
		{
			var item = GameObject.Instantiate(this.sample);
			item.parent = layout.transform;
			item.transform.localScale = Vector2.one;
			var p = item.localPosition;
			p.z = 0;
			item.localPosition = p;
			OnSetData?.Invoke(item, data);
			item.gameObject.SetActive(true);
			itemList.Add(item);
		}
		sample.gameObject.SetActive(false);
	}

	public List<Transform> ItemList
	{
		get
		{
			return this.itemList;
		}
	}


	public event Action DragEnd;
	public void OnEndDrag(PointerEventData eventData)
	{
		DragEnd?.Invoke();
	}

	

}
