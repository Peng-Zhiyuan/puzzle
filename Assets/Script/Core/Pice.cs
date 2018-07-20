﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Pice : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
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
	public BoxCollider2D collider;
	public int index;
	public int indexX;
	public int indexY;

	public List<LinkInfo> linking = new List<LinkInfo>();

	public static int MASK_PIXEL_WIDTH = 300;
	public static int MASK_PIXEL_HIGHT = 300;

	public bool dealedFlag;

	public int boardX = -1;
	public int boardY = -1;
	public int SideIndex = -1;
	public int cellWidth;
	public int cellHeight;

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
		var cellCenterX = map.GetCellCenterX(indexX);
		var cellCenterY = map.GetCellCenterY(indexY);
		var expandedCellWidth = map.cellWidth + map.expand * 2;
		var expandedCellHight = map.cellHeight + map.expand * 2;

		var left = cellCenterX - expandedCellWidth/2;
		var bottom = cellCenterY - expandedCellHight/2;
		var right = left + expandedCellWidth;
		var top = bottom + expandedCellHight;
		

		var sprite = Sprite.Create(map.texture, new Rect(left, bottom, expandedCellWidth, expandedCellHight), new Vector2(0.5f, 0.5f), 1);

		spriteRanderer.sprite = sprite;
		this.MaskWidth = expandedCellWidth;
		this.MaskHight = expandedCellHight;
		this.LeftType = EdgeType.AO;
		this.RightType = EdgeType.AO;
		this.TopType = EdgeType.AO;
		this.BottomType = EdgeType.AO;
		this.collider.size = new Vector2(expandedCellWidth, expandedCellHight);
		this.index = index;
		this.indexX = indexX;
		this.indexY = indexY;
		this.cellWidth = map.cellWidth;
		this.cellHeight = map.cellHeight;

		// set edge
		var info = map.piceInfo[indexX, indexY];
		LeftType = info.left;
		RightType = info.right;
		TopType = info.top;
		BottomType = info.bottom;
	}

	public float MaskWidth
	{
		set
		{
			var scale = value / MASK_PIXEL_WIDTH;
			mask.localScale = new Vector2(scale, mask.localScale.y);
		}
	}

	public float MaskHight
	{
		set
		{
			var scale = value / MASK_PIXEL_HIGHT;
			mask.localScale = new Vector2(mask.localScale.x, scale);
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
	
	private void RemoveFromOwner()
	{
		if(this.owner == PiceOwner.Board)
		{
			Puzzle.instance.board.RemoveWithLinking(this, this.boardX, this.boardY);
		}
		else if(this.owner == PiceOwner.Side)
		{
			Puzzle.instance.side.Remove(this);
		}
		this.owner = PiceOwner.Floating;
	}

	public void SetToSide(int index)
	{
		RemoveFromOwner();
		if(index != -1)
		{
			Puzzle.instance.side.Insert(this, index);
		}
		else
		{
			Puzzle.instance.side.Append(this);
		}
	}

	public void SetToBoard(int indexX, int indexY)
	{
		RemoveFromOwner();
		Puzzle.instance.board.PutWithLinking(this, indexX, indexY);
	}

	public void SetToFloating()
	{
		RemoveFromOwner();
		this.owner = PiceOwner.Floating;
	}

	public void SmoothSetPosition(float x, float y)
	{
		var p = this.transform.position;
		var dx = x - p.x;
		var dy = y - p.y;
		SmoothMovePosition(dx, dy);
	}

	public void SetPostion(float x, float y)
	{
		var p = this.transform.position;
		var dx = x - p.x;
		var dy = y - p.y;
		MovePosition(dx, dy);
	}

	public void MovePosition(float dx, float dy)
	{
		var p = this.transform.position;
		var pp = new Vector2(p.x + dx, p.y + dy);
		this.transform.position = pp;

		ForeachPice(pice =>{
			pice.MovePosition(dx, dy);
		});
	}


	public void SmoothMovePosition(float dx, float dy)
	{
		//GameObject.Destroy(this.gameObject.GetComponent<iTween>());
		iTween.MoveBy(this.gameObject, iTween.Hash("x", dx, "y", dy, "easeType", iTween.EaseType.easeOutCirc, "time", 0.2f));
		ForeachPice(pice =>{
			pice.SmoothMovePosition(dx, dy);
		});
	}

	public void AnimateScale(float scale)
	{
		//GameObject.Destroy(this.gameObject.GetComponent<iTween>());
		iTween.ScaleTo(this.gameObject, new Vector2(scale, scale), 0.4f);
	}

	public void ForeachPice(Action<Pice> callback)
	{
		dealedFlag = true;
		linking.ForEach(info =>{
			if(!info.pice.dealedFlag)
			{
				callback(info.pice);
			}
		});
		dealedFlag = false;
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
		draging = true;
		DraggedInstance = gameObject;
		_startPosition = transform.position;
		_zDistanceToCamera = Mathf.Abs (_startPosition.z - Camera.main.transform.position.z);

		_offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint (
			new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
		);

		dealedFlag = true;
		linking.ForEach(info=>{
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
		linking.ForEach(info=>{
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
		linking.ForEach(info=>{
			if(!info.pice.dealedFlag)
			{
				info.pice.EndDrag();
			}
		});
		dealedFlag = false;
	}

	#endregion
}

public enum EdgeType
{
	AO,
	TU,
	PING,
}