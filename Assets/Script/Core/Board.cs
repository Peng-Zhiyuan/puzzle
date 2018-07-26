using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public SpriteRenderer sr_border;
	int width;
	int height;
	public int xCount;
	public int yCount;
	int maxIndex;
	float cellWidth;
	float cellHeight;

	SpriteRenderer test;
	public PiceStack[,] data;
	public void Init(int width, int height, int xCount, int yCount)
	{
		this.width = width;
		this.height = height;
		this.xCount = xCount;
		this.yCount = yCount;
		this.maxIndex = xCount * yCount - 1;
		this.cellWidth = width / xCount;
		this.cellHeight = height / yCount;

		if(Puzzle.DEBUG)
		{
			this.test = this.transform.Find("test").GetComponent<SpriteRenderer>();
			for(int i = 0; i <= maxIndex; i++)
			{
				var indexX = IndexToIndexX(i);
				var indexY = IndexToIndexY(i);
				var centerX = GetCenterX(indexX);
				var centerY = GetCenterY(indexY);
				var dot = GameObject.Instantiate(this.test);
				dot.transform.parent = this.transform;
				dot.transform.position = new Vector2(centerX, centerY);
			}
		}


		data = new PiceStack[xCount, yCount];
		for(int i = 0; i < xCount; i++)
		{
			for(int j = 0; j < yCount; j++)
			{
				data[i, j] = new PiceStack();
			}
		}
		// set border size
		sr_border.size = new Vector2(width, height);
	}

	public Pice GetPiceFromData(int IndexX, int indexY)
	{
		if(IndexX >= 0 && IndexX < xCount)
		{
			if(indexY >= 0 && indexY < yCount)
			{
				var stack = data[IndexX, indexY];
				if(stack.Count > 0)
				{
					return stack.Peek();
				}
				else
				{
					return null;
				}
			}
		}
		return null;
	}
	public void PlacePice(Pice pice)
	{
		var px = pice.transform.position.x;
		var py = pice.transform.position.y;
		var minCenterX = 0f;
		var minCenterY = 0f;
		var minDistanceSquare = float.MaxValue;
		var minIndex = -1;
		for(var index = 0; index <= maxIndex; index++)
		{
			var indexX = IndexToIndexX(index);
			var indexY = IndexToIndexY(index);
			var centerX = GetCenterX(indexX);
			var centerY = GetCenterY(indexY);
			var distanceSquare = Mathf.Pow(px - centerX, 2) + Mathf.Pow(py - centerY, 2);
			if(distanceSquare < minDistanceSquare)
			{
				minDistanceSquare = distanceSquare;
				minCenterX = centerX;
				minCenterY = centerY;
				minIndex = index;
			}
		}
		pice.SmoothSetPosition(minCenterX, minCenterY); 
		//set data
		var indexX_ = IndexToIndexX(minIndex);
		var indexY_ = IndexToIndexY(minIndex);
		pice.SetToBoard(indexX_, indexY_);
	}

	public bool isIndexInvalid(int indexX, int indexY)
	{
		if(indexX >= 0 && indexX < xCount)
		{
			if(indexY >= 0 && indexY < yCount)
			{
				return true;
			}
		}
		return false;
	}

	private void RemovePiceFromData(Pice pice, int indexX, int indexY)
	{
		var stack = data[indexX, indexY];
		if(stack.Contains(pice))
		{
			stack.Remove(pice);
		}
	}

	private void Push(int indexX, int indexY, Pice pice)
	{
		var stack = this.data[indexX, indexY];
		stack.Push(pice);
		pice.owner = PiceOwner.Board;
		pice.boardX = indexX;
		pice.boardY = indexY;
	}

	private bool Contains(int indexX, int indexY, Pice pice)
	{
		var stack = data[indexX, indexY];
		return stack.Contains(pice);
	}

	private void Remove(Pice pice, int indexX, int indexY)
	{
		if(isIndexInvalid(indexX, indexY))
		{
			data[indexX, indexY].Remove(pice);
		}
		pice.owner = PiceOwner.Floating;
	}

	public void RemoveWithLinking(Pice pice, int indexX, int indexY)
	{
		Remove(pice, indexX, indexY);
		pice.dealedFlag = true;
		pice.linking.ForEach(info =>{
			if(!info.pice.dealedFlag)
			{
				var x = indexX;
				var y = indexY;
				switch(info.directory)
				{
					case LinkDirectory.Top:
						y++;
						break;
					case LinkDirectory.Bottom:
						y--;
						break;
					case LinkDirectory.Left:
						x--;
						break;
					case LinkDirectory.Right:
						x++;
						break;
				}
				RemoveWithLinking(info.pice, x, y);
			}
		});
		pice.dealedFlag = false;
	}

	public void PutWithLinking(Pice pice, int indexX, int indexY)
	{
		if(isIndexInvalid(indexX, indexY))
		{
			this.Push(indexX, indexY, pice);
			pice.boardX = indexX;
			pice.boardY = indexY;
		}
		else
		{
			pice.boardX = -1;
			pice.boardY = -1;
		}
		pice.dealedFlag = true;
		pice.linking.ForEach(info =>{
			if(!info.pice.dealedFlag)
			{
				var x = indexX;
				var y = indexY;
				switch(info.directory)
				{
					case LinkDirectory.Top:
						y++;
						break;
					case LinkDirectory.Bottom:
						y--;
						break;
					case LinkDirectory.Left:
						x--;
						break;
					case LinkDirectory.Right:
						x++;
						break;
				}
				PutWithLinking(info.pice, x, y);
			}
		});
		pice.dealedFlag = false;
	}

	public int IndexToIndexX(int index)
	{
		return index % this.xCount;
	}

	public int IndexToIndexY(int index)
	{
		return index / this.xCount;
	}

	public float GetCenterX(int indexX)
	{
		return Left + indexX * this.cellWidth + this.cellWidth/2;
	}

	public float GetCenterY(int indexY)
	{
		return Bottom + indexY * this.cellHeight + this.cellHeight/2;
	}


	public float Left
	{
		get
		{
			return this.transform.position.x - this.width/2;
		}
	}

	public float Right
	{
		get
		{
			return this.transform.position.x + this.width/2;
		}
	}

	public float Bottom
	{
		get
		{
			return this.transform.position.y - this.height/2;
		}
	}

	public float Top
	{
		get
		{
			return this.transform.position.y + this.height/2;
		}
	}

	public Rect Rect
	{
		get
		{
			return new Rect(Left, Bottom, Right - Left, Top - Bottom);
		}
	}

}
