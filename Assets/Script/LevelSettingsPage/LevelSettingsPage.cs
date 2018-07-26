using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomLitJson;

public class LevelSettingsPage : Page
{
	public SamplizeScrollRect sampleizeScrollRect;
	public RectTransform selectBg;
	public RectTransform selection_norotation;
	public RectTransform selection_rotation;

	public void OnRotateSelection()
	{
		selection_norotation.GetComponent<Image>().enabled = false;
		selection_rotation.GetComponent<Image>().enabled = true;
	}

	public void OnNonRotationSelection()
	{
		selection_norotation.GetComponent<Image>().enabled = true;
		selection_rotation.GetComponent<Image>().enabled = false;
	}

	public override void OnCreate()
	{
		sampleizeScrollRect.OnSetData = OnSetData;
		sampleizeScrollRect.DragEnd += OnDragEnd;
	}

	public override void OnPush()
	{
		// read slice type from static data
		var sheet = StaticDataLite.GetSheet("pice_slice");
		var rowList = new List<JsonData>();
		foreach(string id in sheet.Keys)
		{
			var row = sheet[id];
			rowList.Add(row);
			//dataList.Add("x" + row["cell_size"]);
		}

		sampleizeScrollRect.ChangeData(rowList.ToArray());
		// resize content
		var layout = sampleizeScrollRect.layout as HorizontalLayoutGroup;
		var ItemListLength = rowList.Count * sampleizeScrollRect.sample.rect.width + (rowList.Count - 1) * layout.spacing;
		var preExtra = 500;
		var postExtra = 500;
		var contentLength = preExtra + ItemListLength + postExtra;
		sampleizeScrollRect.Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentLength);
		
		OnRotateSelection();
		lostTime = 0;
		first = true;
	}

	private void OnSetData(Transform itemTr, object dataObj)
	{
		var item = itemTr.GetComponent<LevelSettingsPage_Item>();
		var row = dataObj as JsonData;
		var cellSize = row["cell_size"];
		item.text.text = "x" + cellSize;
		item.dataRow = row;
	}

    public void OnDragEnd()
	{
		TweenNearestToCenter();
	}

	public void TweenNearestToCenter()
	{
		// 计算最近的 item 的 rect
		var itemList = sampleizeScrollRect.ItemList;
		var scrollRectTransform = sampleizeScrollRect.GetComponent<RectTransform>();
		var scrollRectWorldRect = RectTransformUtil.GetWorldRect(scrollRectTransform);
		var scrollRectCenter = scrollRectWorldRect.center;
		var lockPoint = scrollRectCenter;
		
		var shotestDistance = float.MaxValue;
		Vector2 nearestItemCenter = Vector2.zero;
		var nearestItemIndex = -1;
		for(var i = 0; i < itemList.Count; i++)
		{
			var item = itemList[i];
			var rect = RectTransformUtil.GetWorldRect(item.GetComponent<RectTransform>());
			var center = rect.center;
			var distanceP2 = MathUtil.PointDistancePower2(lockPoint, center);
			if(distanceP2 < shotestDistance)
			{
				shotestDistance = distanceP2;
				nearestItemCenter = center;
				nearestItemIndex = i;
			}
		}

		// 计算移动向量
		var moveVector = lockPoint - nearestItemCenter;
		var moveVector3 = new Vector3(moveVector.x, moveVector.y);
		// tween scrollRect
		var content = sampleizeScrollRect.GetComponent<ScrollRect>().content;
		//iTween.MoveBy(content.gameObject, moveVector, 0.2f);
		iTween.MoveBy(content.gameObject, iTween.Hash("amount", moveVector3, "time", 0.2f, "easetype", "easeOutQuad"));

		Debug.Log(nearestItemIndex);
		Debug.Log(moveVector);
	}

	float lostTime = 0;
	bool first = true;
	LevelSettingsPage_Item selectItem;
	public void Update()
	{
		// tween center at frame 2
		lostTime += Time.deltaTime;

		if(lostTime >= 0.3f)
		{
			if(first)
			{
				
				TweenNearestToCenter();
				first = false;
			}
		}

		

		// 计算离中心点最近的 item
		var itemList = sampleizeScrollRect.ItemList;
		var scrollRectTransform = sampleizeScrollRect.GetComponent<RectTransform>();
		var scrollRectWorldRect = RectTransformUtil.GetWorldRect(scrollRectTransform);
		var scrollRectCenter = scrollRectWorldRect.center;
		var lockPoint = scrollRectCenter;
		
		var shotestDistance = float.MaxValue;
		Vector2 nearestItemCenter = Vector2.zero;
		var nearestItemIndex = -1;
		for(var i = 0; i < itemList.Count; i++)
		{
			var item = itemList[i];
			var rect = RectTransformUtil.GetWorldRect(item.GetComponent<RectTransform>());
			var center = rect.center;
			var distanceP2 = MathUtil.PointDistancePower2(lockPoint, center);
			if(distanceP2 < shotestDistance)
			{
				shotestDistance = distanceP2;
				nearestItemCenter = center;
				nearestItemIndex = i;
			}
		}

		// 选中这个 item
		for(var i = 0; i < itemList.Count; i++)
		{
			var item = itemList[i];
			var itemComp = item.GetComponent<LevelSettingsPage_Item>();
			if(i == nearestItemIndex)
			{
				itemComp.SetBig();
				selectItem = itemComp;
			}
			else
			{
				itemComp.SetSmall();
			}
		}
	}

	public void OnStartButton()
	{
		var picId = int.Parse(this.param);
		GameController.EnterCore(picId, selectItem.dataRow.Get<int>("id"));
	}
}
