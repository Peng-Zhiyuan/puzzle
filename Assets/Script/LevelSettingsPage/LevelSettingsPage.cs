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
	public Button button_continue;
	public Button button_newGame;

	int PicId
	{
		get
		{
			return int.Parse(this.param as string);
		}
	}

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

		// 检查是否有已经开始的拼图
		// var picId = PicId;
		// var b = HasUncompleteGame(picId);
		// if(b)
		// {

		// }
		// if(info == null)
		// {
		// 	button_continue.gameObject.SetActive(false);
		// }
		// else
		// {
		// 	button_continue.gameObject.SetActive(true);
		// }
	}

	private bool HasUncompleteGame(int picId)
	{
		var info = PlayerStatus.TryGetUncompleteOfPicId(picId);
		if(info == null)
		{
			return false;
		}
		return true;
	}

	private void OnSetData(Transform itemTr, object dataObj)
	{
		var item = itemTr.GetComponent<LevelSettingsPage_Item>();
		var row = dataObj as JsonData;
		//var cellSize = row["cell_size"];
		var name = row.Get<string>("name");
		item.text.text = name;
		item.dataRow = row;
	}

    public void OnDragEnd()
	{
		TweenNearestToCenter();
	}

	Vector2 GetNearestItemCenter()
	{
		// 计算最近的 item 的 rect
		var lockPoint = GetScrollRectCenter();

		var itemList = sampleizeScrollRect.ItemList;
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
		Debug.Log(nearestItemIndex);
		return nearestItemCenter;
	}

	Vector2 GetCenterOfItem(int index)
	{
		var itemList = sampleizeScrollRect.ItemList;
		var item = itemList[index];
		var rect = RectTransformUtil.GetWorldRect(item.GetComponent<RectTransform>());
		var center = rect.center;
		return center;
	}

	public Vector2 GetScrollRectCenter()
	{
		var scrollRectTransform = sampleizeScrollRect.GetComponent<RectTransform>();
		var scrollRectWorldRect = RectTransformUtil.GetWorldRect(scrollRectTransform);
		var scrollRectCenter = scrollRectWorldRect.center;
		var lockPoint = scrollRectCenter;
		return lockPoint;
	}

	void TweenScrollContent(Vector2 v)
	{
		var moveVector3 = new Vector3(v.x, v.y);
		var content = sampleizeScrollRect.GetComponent<ScrollRect>().content;
		//iTween.MoveBy(content.gameObject, moveVector, 0.2f);
		iTween.MoveBy(content.gameObject, iTween.Hash("amount", moveVector3, "time", 0.2f, "easetype", "easeOutQuad"));
	}

	public void TweenNearestToCenter()
	{
		var nearestItemCenter = GetNearestItemCenter();
		var lockPoint = GetScrollRectCenter();

		// 计算移动向量
		var moveVector = lockPoint - nearestItemCenter;
		
		// tween scrollRect
		TweenScrollContent(moveVector);

		Debug.Log(moveVector);
	}

	public void TweenItemToCenter(int index)
	{
		var itemCenter = GetCenterOfItem(index);
		var lockPoint = GetScrollRectCenter();

		// 计算移动向量
		var moveVector = lockPoint - itemCenter;
		
		// tween scrollRect
		TweenScrollContent(moveVector);

		Debug.Log(moveVector);
	}

	float lostTime = 0;
	bool first = true;

	LevelSettingsPage_Item _selectItem;
	LevelSettingsPage_Item selectItem
	{
		set
		{
			if(_selectItem == value)
			{
				return;
			}
			_selectItem = value;
			RefreshButton();
			AudioManager.PlaySe("button");
		}
		get
		{
			return _selectItem;
		}
	}
	public void RefreshButton()
	{
		// 检查当前pid和sliceId是否有中断的游戏
		var picId = PicId;
		var sliceId = _selectItem.dataRow.Get<int>("id");
		var b = HasUncompleteGame(picId, sliceId);
		if(b)
		{
			button_continue.gameObject.SetActive(true);
			button_newGame.gameObject.SetActive(false);
		}
		else
		{
			button_continue.gameObject.SetActive(false);
			button_newGame.gameObject.SetActive(true);
		}
	
	}

	private bool HasUncompleteGame(int picId, int sliceId)
	{
		var info = PlayerStatus.TryGetUncompleteOfPicId(picId);
		if(info == null)
		{
			return false;
		}
		if(info.sliceId == sliceId)
		{
			return true;
		}
		return false;
	}

	public void Update()
	{
		// tween center at frame 2
		lostTime += Time.deltaTime;

		if(lostTime >= 0.3f)
		{
			if(first)
			{
				if(!GuideFloating.guideMode)
				{
					var b = HasUncompleteGame(PicId);
					if(b)
					{
						var info = PlayerStatus.TryGetUncompleteOfPicId(PicId);
						var sliceId = info.sliceId;
						var itemIndex = sliceId - 1;
						TweenItemToCenter(itemIndex);
					}
					else
					{
						TweenNearestToCenter();
					}
				}
				else
				{
					TweenItemToCenter(0);
				}

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
		if(first) return;
		// 检查这个 picId 是否已经有存档，如果有则提示
		var picId = int.Parse(this.param as string);

		var info = PlayerStatus.TryGetUncompleteOfPicId(picId);
		if(info != null)
		{
			var param = new DialogParam();
			param.des = "The new game will restore Record. Do you want to continue?";
			param.button = "confirm";
			var admin = new Admission_PopupNewPage();
			var dialog = UIEngine.Forward<DialogPage>(param, admin);
			dialog.Complete = result =>{
				if(result == DialogResult.Conform)
				{
					GameController.EnterCore(picId, selectItem.dataRow.Get<int>("id"));
				}
			};
		}
		else
		{
			GameController.EnterCore(picId, selectItem.dataRow.Get<int>("id"));
		}
		AudioManager.PlaySe("button");
		RadioStation.Brodcast("NEW_GAME");
		
	}

	public void OnContinue()
	{
		if(first) return;
		var picId = int.Parse(this.param as string);
		var info = PlayerStatus.TryGetUncompleteOfPicId(picId);
		GameController.EnterWithInfo(info);
		AudioManager.PlaySe("button");
	}

	public void OnButtonClose()
	{
		HeadBarFloating.instance.OnBackButton();
		AudioManager.PlaySe("button");
	}
}
