using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour 
{
	public ScrollView scrollView;
	public int width;
	public int count
	{
		get
		{
			return list.Count;
		}
	}
	public int cellWidth;
	private Transform prefab_dot;
	public List<Pice> list = new List<Pice>();

	public void Init(int cellWidth)
	{
		this.cellWidth = cellWidth;
		this.prefab_dot = transform.Find("dot");
		list.Clear();
		// for(int i = 0; i < count; i++)
		// {
		// 	var x = GetCellCenterX(i);
		// 	var y = GetCellCenterY(i);
		// 	var dot = GameObject.Instantiate(prefab_dot);
		// 	dot.transform.localPosition = new Vector2(x, y);
		// 	dot.transform.parent = this.transform;
		// }
		if(!Puzzle.DEBUG)
		{
			prefab_dot.gameObject.SetActive(false);
		}
	}


	public void SetContentLength(int length)
	{
		scrollView.SetContentLength(length);
	}

	public void PlacePice(Pice pice)
	{
		var px = pice.transform.position.x;
		var py = pice.transform.position.y;
		var minCenterX = 0f;
		var minCenterY = 0f;
		var minDistanceSquare = float.MaxValue;
		var minIndex = -1;
		for(var index = 0; index < count; index++)
		{
			var centerX = GetCellCenterX(index);
			var centerY = GetCellCenterY(index);
			var distanceSquare = Mathf.Pow(px - centerX, 2) + Mathf.Pow(py - centerY, 2);
			if(distanceSquare < minDistanceSquare)
			{
				minDistanceSquare = distanceSquare;
				minCenterX = centerX;
				minCenterY = centerY;
				minIndex = index;
			}
		}
		//pice.SmoothSetPosition(minCenterX, minCenterY); 
		pice.boardX = -1;
		pice.boardY = -1;
		//Insert(pice, minIndex);
		pice.SetToSide(minIndex);
		//pice.AnimateScale(50/pice.cellWidth);
		RepositionPiceList();
	}

	public void Insert(Pice pice, int index)
	{
		list.Insert(index, pice);
		pice.owner = PiceOwner.Side;
		pice.sideIndex = index;
		pice.transform.parent = scrollView.content;
		SetContentLength(list.Count * cellWidth);
	}

	public void Append(Pice pice)
	{
		list.Add(pice);
		pice.owner = PiceOwner.Side;
		pice.sideIndex = list.Count - 1;
		pice.transform.parent = scrollView.content;
		SetContentLength(list.Count * cellWidth);
	}

	public void Remove(Pice pice)
	{
		list.Remove(pice);
		pice.owner = PiceOwner.Floating;
		pice.transform.parent = null;
		SetContentLength(list.Count * cellWidth);
	}

	public void RepositionPiceList()
	{
		for(int i = 0; i < this.list.Count; i++)
		{
			var pice = this.list[i];
			var x = GetCellCenterX(i);
			var y = GetCellCenterY(i);
			if(!pice.draging)
			{
				pice.SmoothSetPositionWithBlock(x, y);
				var scale = ((float)this.cellWidth / pice.cellWidth) * 0.65f;
				pice.AnimateScale(scale);
			}
		}
	}

	public void RepositionPiceListNoAnimation()
	{
		for(int i = 0; i < this.list.Count; i++)
		{
			var pice = this.list[i];
			var x = GetCellCenterX(i);
			var y = GetCellCenterY(i);
			if(!pice.draging)
			{
				pice.SetPostion(x, y);
				var scale = ((float)this.cellWidth / pice.cellWidth) * 0.65f;
				pice.SetScale(scale);
			}
		}
	}

	public float Top
	{
		get
		{
			return this.transform.position.y + 50;
			//return this.transform.position.y + this.height/2;
		}
	}

	public float Bottom
	{
		get
		{
			return this.transform.position.y - 50;
			//return this.transform.position.y - this.height/2;
		}
	}

	public float Left
	{
		get
		{
			return this.transform.position.x;
			//return this.transform.position.x - 50;
		}
	}

	public float Right
	{
		get
		{
			//return this.transform.position.x + 50;
			return this.transform.position.x + (count + 5) * cellWidth;
		}
	}

	public Rect Rect
	{
		get
		{
			return new Rect(Left, Bottom, Right - Left, Top - Bottom);
		}
	}

	public float GetCellCenterY(int index)
	{
		//return Top - index * cellHeight - cellHeight/2;
		return this.transform.position.y;
	}

	public float GetCellCenterX(int index)
	{
		//return this.transform.position.x;
		return this.transform.position.x + index * cellWidth + cellWidth/2;
	}

	public Rect GetCellRect(int index)
	{
		float left = Left + index * cellWidth;
		float right = left + cellWidth;
		float bottom = Bottom;
		float top = Top;
		return new Rect(left, bottom, right - left, top - bottom);
	}
}
