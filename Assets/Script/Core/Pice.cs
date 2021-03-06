﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Pice : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
	public int id;
	public Transform mask;
	public SpriteMask maskLeft;
	public SpriteMask maskRigjt;
	public SpriteMask maskTop;
	public SpriteMask maskBottom;
	public SpriteRenderer sr_lien_left;
	public SpriteRenderer sr_lien_right;
	public SpriteRenderer sr_lien_top;
	public SpriteRenderer sr_lien_bottom;
	public SpriteRenderer spriteRanderer;
	public Sprite sprite_ao;
	public Sprite sprite_tu;
	public Sprite sprite_ping;
	public SpriteRenderer sr_flash;
	public BoxCollider2D collider;
	/// <summary>
	/// 所在图片上的索引，从左到右，从下到上
	/// </summary>
	public int index;
	/// <summary>
	/// 所在图片上的横索引，从左到右
	/// </summary>
	public int indexX;
	/// <summary>
	/// 所在图片上的纵索引，从下到上
	/// </summary>
	public int indexY;

	public List<Linking> linkingList = new List<Linking>();

	public static int MASK_WIDTH = 300;
	public static int MASK_HIGHT = 300;

	public bool dealedFlag;

	public int boardX = -1;
	public int boardY = -1;
	public int sideIndex = -1;
	public float cellWidth;
	public float cellHeight;

	private bool _isFiexed;
	/// <summary>
	/// 是否已经固定在正确位置上
	/// </summary>
	public bool isFixed
	{
		get
		{
			return _isFiexed;
		}
		set
		{
			if(value == _isFiexed)
			{
				return;
			}
			_isFiexed = value;
			if(value)
			{
				collider.enabled = false;
				this.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = 1;
				
			}
			else
			{
				collider.enabled = true;
			}
		}
	}

	public int SortingOrder
	{
		get
		{
			return this.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder;
		}
		set
		{
			this.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = value;
		}
	}

	public PiceOwner owner;

	public void Init(Map map, int index)
	{
		// var textureWidth = texture.width;
		// var textureHight = texture.height;
		// float cellWidth = textureWidth / xCount;
		// float cellHeight = textureHight / yCount;
		// var cellCount = xCount * yCount;
		// var maxIndex = cellCount - 1;
		var indexX = map.IndexToIndexX(index);
		var indexY = map.IndexToIndexY(index);
		var cellCenterX = map.GetCellPixelCenterX(indexX);
		var cellCenterY = map.GetCellCenterY(indexY);
		var expandedCellPixelWidth = map.cellPixelWidth + map.pixelExpand * 2;
		var expandedCellPixelHight = map.cellPixelHeight + map.pixelExpand * 2;

		var left = cellCenterX - expandedCellPixelWidth/2;
		var bottom = cellCenterY - expandedCellPixelHight/2;
		var right = left + expandedCellPixelWidth;
		var top = bottom + expandedCellPixelHight;
		

		var sprite = Sprite.Create(map.texture, new Rect(left, bottom, expandedCellPixelWidth, expandedCellPixelHight), new Vector2(0.5f, 0.5f), map.pixlesPerUnit);

		spriteRanderer.sprite = sprite;
		var cellWidth = expandedCellPixelWidth / map.pixlesPerUnit;
		var cellHeight = expandedCellPixelHight / map.pixlesPerUnit;
		this.MaskWidth = cellWidth;
		this.MaskHeight = cellHeight;
		this.FlashSize = new Vector2(cellWidth, cellHeight);
		this.LeftType = EdgeType.AO;
		this.RightType = EdgeType.AO;
		this.TopType = EdgeType.AO;
		this.BottomType = EdgeType.AO;
		this.collider.size = new Vector2(cellWidth, cellHeight);
		this.index = index;
		this.indexX = indexX;
		this.indexY = indexY;
		this.cellWidth = cellWidth;
		this.cellHeight = cellHeight;

		// set edge
		var info = map.piceInfo[indexX, indexY];
		LeftType = info.left;
		RightType = info.right;
		TopType = info.top;
		BottomType = info.bottom;

		this.sr_flash.enabled = false;
	}

	public float MaskWidth
	{
		set
		{
			var scale = value / MASK_WIDTH;
			mask.localScale = new Vector2(scale, mask.localScale.y);
		}
	}

	public float MaskHeight
	{
		set
		{
			var scale = value / MASK_HIGHT;
			mask.localScale = new Vector2(mask.localScale.x, scale);
		}
	}


	public Vector2 FlashSize
	{
		set
		{
			sr_flash.size = value;
		}
	}

	private Sprite EdgetTypeToMaskSprite(EdgeType type)
	{
		if(type == EdgeType.AO)
		{
			return sprite_ao;
		}
		else if(type == EdgeType.TU)
		{
			return sprite_tu;
		}
		else if(type == EdgeType.PING)
		{
			return sprite_ping;
		}
		throw new Exception("unsupport edgeType: " + type);
	}

	[ContextMenu("ChangeLeftType")]
	public void ChangeLeftType()
	{
		var type = LeftType;
		if(type == EdgeType.AO)
		{
			type = EdgeType.TU;
		}
		else if(type == EdgeType.TU)
		{
			type = EdgeType.AO;
		}
		LeftType = type;
	}

	private EdgeType _leftType;
	public EdgeType LeftType
	{
		set
		{
			maskLeft.sprite = EdgetTypeToMaskSprite(value);
			_leftType = value;
			sr_lien_left.gameObject.SetActive(value == EdgeType.PING);
		}
		get
		{
			return _leftType;
		}
	}

	private EdgeType _rightType;
	public EdgeType RightType
	{
		set
		{
			maskRigjt.sprite = EdgetTypeToMaskSprite(value);
			_rightType = value;
			sr_lien_right.gameObject.SetActive(value == EdgeType.PING);
		}
		get
		{
			return _rightType;
		}
	}

	private EdgeType _bottomType;
	public EdgeType BottomType
	{
		set
		{
			maskBottom.sprite = EdgetTypeToMaskSprite(value);
			_bottomType = value;
			sr_lien_bottom.gameObject.SetActive(value == EdgeType.PING);
		}
		get
		{
			return _bottomType;
		}
	}

	private EdgeType _topType;
	public EdgeType TopType
	{
		set
		{
			maskTop.sprite = EdgetTypeToMaskSprite(value);
			_topType = value;
			sr_lien_top.gameObject.SetActive(value == EdgeType.PING);
		}
		get
		{
			return _topType;
		}
	}

	public void SetToFloating()
	{
		PiceMover.SetBlockToFloat(this);//RemoveFromOwner();
		this.owner = PiceOwner.Floating;
	}

	public void TweenToPosition(float x, float y)
	{
		iTween.MoveTo(this.gameObject, iTween.Hash("x", x, "y", y, "easeType", iTween.EaseType.easeOutCirc, "time", 0.2f));
	}

	public void TweenBlockToPosition(float x, float y)
	{
		var p = this.transform.position;
		var dx = x - p.x;
		var dy = y - p.y;
		ForeachPiceOfBlock(pice=>{
			iTween.MoveBy(pice.gameObject, iTween.Hash("x", dx, "y", dy, "easeType", iTween.EaseType.easeOutCirc, "time", 0.2f));
		});
	}

	public void SetPostion(float x, float y)
	{
		this.transform.position = new Vector2(x, y);
	}

	public void SetBlockPosition(float x, float y)
	{
		var p = this.transform.position;
		var dx = x - p.x;
		var dy = y - p.y;
		ForeachPiceOfBlock(pice=>{
			var pp = pice.transform.position;
			var ppp = new Vector2(p.x + dx, p.y + dy);
			pice.transform.position = ppp;
		});
	}



	public void StopTween()
	{
		iTween.Stop(this.gameObject);
	}

	public void TweenToScale(float scale)
	{
		//GameObject.Destroy(this.gameObject.GetComponent<iTween>());
		iTween.ScaleTo(this.gameObject, new Vector2(scale, scale), 0.4f);
	}

	public void SetScale(float scale)
	{
		//GameObject.Destroy(this.gameObject.GetComponent<iTween>());
		this.gameObject.transform.localScale = new Vector2(scale, scale);
	}

	/// <summary>
	/// 遍历被这个 pice 连结到的 pice (包括自身)
	/// </summary>
	/// <param name="callback"></param>
	public void ForeachPiceOfBlock(Action<Pice> callback)
	{
		var dic = DicPool.Take();
		_ForeachPickOfBlock(dic, callback);
		DicPool.Put(dic);
	}

	public void _ForeachPickOfBlock(Dictionary<Pice, bool> dic, Action<Pice> callback)
	{
		if(dic.ContainsKey(this) && dic[this])
		{
			return;
		}
		dic[this] = true;
		callback(this);
		linkingList.ForEach(info =>{
			info.pice._ForeachPickOfBlock(dic, callback);
		});
	}

	public void ForeachPiceOfBlockWhitShift(Action<Pice, Vector2Int> callback)
	{
		var dic = DicPool.Take();
		_ForeachPickOfBlockWithShift(dic, new Vector2Int(0, 0), callback);
		DicPool.Put(dic);
	}

	public void _ForeachPickOfBlockWithShift(Dictionary<Pice, bool> dic, Vector2Int shift, Action<Pice, Vector2Int> callback)
	{
		if(dic.ContainsKey(this) && dic[this])
		{
			return;
		}
		dic[this] = true;
		callback(this, shift);
		linkingList.ForEach(info =>{
			var nextShift = shift;
			switch(info.directory)
			{
				case LinkDirectory.Top:
					nextShift.y ++;
					break;
				case LinkDirectory.Bottom:
					nextShift.y --;
					break;
				case LinkDirectory.Left:
					nextShift.x --;
					break;
				case LinkDirectory.Right:
					nextShift.x ++;
					break;
				default:
					throw new Exception("unsupport directory" + info.directory);
			}
			info.pice._ForeachPickOfBlockWithShift(dic, nextShift, callback);
		});
	}

	public void Flash()
	{
		this.sr_flash.enabled = true;
		sr_flash.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
		iTween.ColorFrom(sr_flash.gameObject, iTween.Hash("color", new Color(1.0f, 1.0f, 1.0f, 1.0f), "time", 0.2f ));
	}

	public void FlashAsLink()
	{
		Flash();
		AudioManager.PlaySe("pice-link");
	}

	public void FlashAsFix()
	{
		Flash();
		AudioManager.PlaySe("pice-link");
	}

 	#region Interface Implementations
 
	public void OnPointerDown(UnityEngine.EventSystems.PointerEventData data)
	{
		if(this.owner == PiceOwner.Side)
		{
			Puzzle.instance.side.scrollView.OnTouch();
		}
	}

  	public GameObject DraggedInstance;
  	Vector3 _startPosition;
	Vector3 _offsetToMouse;
	float _zDistanceToCamera;
	public bool draging;
	public Vector3 _startInput;
	public bool sendToSide;
	public bool needDecideSendToSide;

	public void OnBeginDrag (PointerEventData eventData)
	{
		_startInput = Input.mousePosition;
		needDecideSendToSide = true;
		BeginDrag(eventData);
	}

	private void BeginDrag(PointerEventData eventData)
	{
		// 已经 fixed 的 pice 无法拖动
		if(this.isFixed)
		{
			return;
		}

		// 给这个 pice 分配最高的 layer
		this.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingOrder = LayerOrderDispatcher.Next;

		draging = true;
		DraggedInstance = gameObject;
		_startPosition = transform.position;
		_zDistanceToCamera = Mathf.Abs (_startPosition.z - Camera.main.transform.position.z);

		_offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint (
			new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
		);

		dealedFlag = true;
		linkingList.ForEach(info=>{
			if(!info.pice.dealedFlag)
			{
				info.pice.BeginDrag(eventData);
			}
		});
		dealedFlag = false;
	}

	public void OnDrag (PointerEventData eventData)
	{
		if(needDecideSendToSide)
		{
			if(this.owner == PiceOwner.Side)
			{
				needDecideSendToSide = false;

				var nowInput = Input.mousePosition;
				var deltaX = Math.Abs(nowInput.x - _startInput.x);
				var deltaY = Math.Abs(nowInput.y - _startInput.y);
				if(deltaX == 0 && deltaY == 0)
				{
					needDecideSendToSide = true;
					return;
				}

				if(deltaX > deltaY)
				{
					this.sendToSide = true;
				}
				else
				{
					this.sendToSide = false;
				}
				if(this.sendToSide)
				{
					this.EndDrag();
					Puzzle.instance.side.scrollView.OnBeginDrag(eventData);
				}
			}
			else
			{
				needDecideSendToSide = false;
				this.sendToSide = false;
			}

		}
		if(!sendToSide)
		{
			Puzzle.instance.OnRootPiceDraging(this);
			Drag(eventData);
		}
		else
		{
			Puzzle.instance.side.scrollView.OnDrag(eventData);
		}
	}


	private void Drag(PointerEventData eventData)
	{
		if(!draging)
		{
			return;
		}
		if(Input.touchCount > 1)
			return;


		transform.position = Camera.main.ScreenToWorldPoint (
			new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
			) + _offsetToMouse;

		dealedFlag = true;
		linkingList.ForEach(info=>{
			if(!info.pice.dealedFlag)
			{
				info.pice.Drag(eventData);
			}
		});
		dealedFlag = false;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if(sendToSide)
		{
			Puzzle.instance.side.scrollView.OnEndDrag(eventData);
		}
		else
		{
			EndDrag();
			Puzzle.instance.OnRootPiceDragEnd(this);
		}
	}

	public void EndDrag ()
	{
		draging = false;
		DraggedInstance = null;
		_offsetToMouse = Vector3.zero;

		dealedFlag = true;
		linkingList.ForEach(info=>{
			if(!info.pice.dealedFlag)
			{
				info.pice.EndDrag();
			}
		});
		dealedFlag = false;
	}

	#endregion

	public PiceInfo CreateInfo()
	{
		var info = new PiceInfo
		{
			index = this.index,
			boardX = this.boardX,
			boardY = this.boardY,
			sideIndex = this.sideIndex,
			isFixed = this.isFixed,
			owner = this.owner,
			leftType = this.LeftType,
			rightType = this.RightType,
			bottomType = this.BottomType,
			topType = this.TopType,
			sortingOrder = this.SortingOrder,
		};
		foreach(var l in linkingList)
		{
			var linkingInfo = l.CreateInfo();
			info.LinkingInfoList.Add(linkingInfo);
		}
		return info;
	}
}

public enum EdgeType
{
	AO,
	TU,
	PING,
}
