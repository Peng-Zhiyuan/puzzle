using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map 
 {
	public Texture2D texture;
	public int validPixelHeight;
	public int validPixelWidth;
	public int cellPixelWidth;
	public int cellPixelHeight;
	public int cellCount;
	public int pixelExpand;
	public int xCount;
	public int yCount;
	public int cellPixelSize;

	public Rect validRect;
	public PiceEdgeInfo[,] piceInfo;
	public Sprite validSprite;

	public float pixlesPerUnit;
	public float validHeight;
	public float validWidth;
	public float cellHeigth;
	public float cellWidth; 
	public float expand;

	public void Init(Texture2D texture, int pixelExpand, int cellPixelSize)
	{
		this.cellPixelSize = cellPixelSize;
		this.texture = texture;
		var realWidth = texture.width - 2 * pixelExpand;
		var readlHeight = texture.height - 2 * pixelExpand;
		var xCount = realWidth / cellPixelSize;
		var yCount = readlHeight / cellPixelSize;
		var validWidth = xCount * cellPixelSize;
		var validHeight = yCount * cellPixelSize;
		var rectLeft = (texture.width - validWidth)/2;
		var rectBottom = (texture.height - validHeight)/2;
		var validRect = new Rect(rectLeft, rectBottom, validWidth, validHeight);


		this.validRect = validRect;
		this.validPixelWidth = validWidth;
		this.validPixelHeight = validHeight;
		this.cellPixelWidth = cellPixelSize;
		this.cellPixelHeight = cellPixelSize;
		this.cellCount = xCount * yCount;
		this.pixelExpand = pixelExpand;
		this.xCount = xCount;
		this.yCount = yCount;
		GeneratePiceShap();

		pixlesPerUnit = realWidth / 1000f;
		this.validWidth = validPixelWidth / pixlesPerUnit;
		this.validHeight = validPixelHeight / pixlesPerUnit;
		this.cellWidth = cellPixelWidth / pixlesPerUnit;
		this.cellHeigth = cellPixelHeight / pixlesPerUnit;
		this.expand = this.pixelExpand / pixlesPerUnit;

		//Debug.Log("validRect:" + validRect);
		validSprite = Sprite.Create(this.texture, validRect, new Vector2(0.5f, 0.5f), pixlesPerUnit);
	}


	public class PiceEdgeInfo
	{
		public EdgeType top;
		public EdgeType bottom;
		public EdgeType left;
		public EdgeType right;
	}

	public void GeneratePiceShap()
	{
		piceInfo = new PiceEdgeInfo[xCount, yCount];
		for(int i = 0; i < xCount; i++)
		{
			for(int j = 0; j < yCount; j++)
			{
				var x = i;
				var y = j;
				var info = new PiceEdgeInfo();
				// decide left
				if(x == 0)
				{
					info.left = EdgeType.PING;
				}
				else
				{
					// 根据左边决定
					var leftInfo = piceInfo[x-1, y];
					info.left = GetReverse(leftInfo.right);
				}
				// decide right
				if(x == xCount - 1)
				{
					info.right = EdgeType.PING;
				}
				else
				{
					// 随机
					info.right = RandomAoTu();
				}
				// decide bottom
				if(y == 0)
				{
					info.bottom = EdgeType.PING;
				}
				else
				{
					// 根据下边决定
					var bototmInfo = piceInfo[x, y-1];
					info.bottom = GetReverse(bototmInfo.top);
				}
				// decide top
				if(y == yCount - 1)
				{
					info.top = EdgeType.PING;
				}
				else
				{
					// 随机
					info.top = RandomAoTu();
				}
				piceInfo[x, y] = info;
			}
		}
	}

	public EdgeType GetReverse(EdgeType edgeType)
	{
		switch(edgeType)
		{
			case EdgeType.AO:
				return EdgeType.TU;
			case EdgeType.TU:
				return EdgeType.AO;
			case EdgeType.PING:
				return EdgeType.PING;
		}
		throw new System.Exception("unsupport edgeType: " + edgeType);
	}

	private EdgeType RandomAoTu()
	{
		var value = (int)(UnityEngine.Random.value * 100);
		var tu = value % 2 == 0;
		if(tu)
		{
			return EdgeType.TU;
		}
		else
		{
			return EdgeType.AO;
		}
	}

	public bool IsValid(int IndexX, int indexY)
	{
		if(IndexX >= 0 && IndexX < xCount)
		{
			if(indexY >=0 && indexY < yCount)
			{
				return true;
			}
		}
		return false;
	}

	// public void Init(Texture2D texture, int expand, int xCount, int yCount)
	// {
	// 	this.texture = texture;
	// 	this.validHeight = texture.width - 2 * expand;
	// 	this.validHeight = texture.height - 2 * expand;
	// 	this.cellWidth = validHeight / xCount;
	// 	this.cellHeight = validHeight / yCount;
	// 	this.cellCount = xCount * yCount;
	// 	this.expand = expand;
	// 	this.xCount = xCount;
	// 	this.yCount = yCount;
	// }

	public float GetCellPixelCenterX(int indexX)
	{
		return validRect.xMin + indexX * cellPixelWidth + cellPixelWidth/2;
		//return expand + indexX * cellWidth + cellWidth/2;
	}

	public float GetCellCenterY(int indexY)
	{
		return validRect.yMin + indexY * cellPixelHeight + cellPixelHeight/2;
		//return expand + indexY * cellHeight + cellHeight/2;
	}

	public int IndexToIndexX(int index)
	{
		return index % xCount;
	}

	public int IndexToIndexY(int index)
	{
		return index / xCount;
	}

}
