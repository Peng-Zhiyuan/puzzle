using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Puzzle 
{
	public event Action Complete;

	public static int EXPAND = 10;
	public Texture2D expanedTexture;

	public Board board;
	public Side side;

	public static Puzzle instance;
	public static Puzzle Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new Puzzle();
			}
			return instance;
		}
	}
	public Pice prefab_pice;
	private Core core;
	private Map map;


	public void Init () {
		instance = this;
		UnityEngine.Random.InitState(DateTime.UtcNow.Second);
		LayerOrderDispatcher.Init();
		var core_go = GameObject.Find("Core");
		if(core_go == null)
		{
			var prefab_core = Resources.Load<Core>("Core/Core");
			this.core = GameObject.Instantiate(prefab_core);
		}
		else
		{
			this.core = core_go.GetComponent<Core>();
		}
		this.prefab_pice = Resources.Load<Pice>("Core/Pice");
		this.board = core.board;
		this.side = core.side;
	}

	private Texture2D ExpandTexture(Texture2D texture)
	{
		// 扩张 texture
		var t = new Texture2D(texture.width + 2 * EXPAND, texture.height + 2 * EXPAND);
		var w = texture.width;
		var h = texture.height;
		for(int i = 0; i < w; i++)
		{
			for(int j = 0; j < h; j++)
			{
				var pixel = texture.GetPixel(i, j);
				t.SetPixel(i + EXPAND, j + EXPAND, pixel);
			}
		}
		t.Apply();
		return t;
	}

	public void StartPuzzle(Texture2D texture, int cellSize)
	{
		EXPAND = (int)(cellSize * 0.25f);
		expanedTexture = ExpandTexture(texture);
		map = new Map();
		map.Init(expanedTexture, EXPAND, cellSize);

		board.Init(map.validWidth, map.validHeight, map.xCount, map.yCount);
		side.Init(100);
		//side.Init(map.validHeight, map.yCount);
		var count = map.xCount * map.yCount;
		for(int i = 0; i < count; i++)
		{
			var pice = GameObject.Instantiate<Pice>(prefab_pice);
			pice.Init(map, i);
			side.Append(pice);
		}
		side.RepositionPiceList();
	}



	public void OnRootPiceDraging(Pice pice)
	{
		UpdatePiceOwner(pice);
	}

	public void UpdatePiceOwner(Pice pice)
	{
		if(pice.owner == PiceOwner.Side)
		{
			var inSide = side.Rect.Contains(pice.transform.position);
			if(!inSide)
			{
				// 认为从side中拖出
				side.Remove(pice);
				side.RepositionPiceList();
				pice.AnimateScale(1);
				side.scrollView.AnimateFixContentPosition();
			}
			else
			{
				var rect = side.GetCellRect(pice.SideIndex);
				if(!rect.Contains(pice.transform.position))
				{
					// 认为在side中移动了位置
					side.Remove(pice);
					side.PlacePice(pice);
				}
			}
		}
		else if(pice.owner != PiceOwner.Side)
		{
			var inSide = side.Rect.Contains(pice.transform.position);
			if(inSide)
			{
				// 认为从拖入side
				side.PlacePice(pice);
				side.RepositionPiceList();
			}
		}
	}

	public void OnRootPiceDragEnd(Pice pice)
	{
		UpdatePiceOwner(pice);
		// 没有连结的 pice， 当拖放释放点不在 board 上时，自动回到 side 上
		if(pice.linking.Count == 0)
		{
			var inboard = board.Rect.Contains(pice.transform.position);
			if(!inboard)
			{
				side.PlacePice(pice);
				return;
			}
		}

		// 应该放置到 board 上时
		board.PlacePice(pice);
		CheckNewLink();
		CheckNewFix();
		var complete = IsAllPiceFixed();
		if(complete)
		{
			Debug.Log("[Core] Complete");
			CoroutineManager.Create(waiteAndSendComplete());
		}
		
	}

	private IEnumerator waiteAndSendComplete()
	{
		yield return new WaitForSeconds(0.4f);
		Complete?.Invoke();
	}

	private bool SameRow(Pice pice1, Pice pice2)
	{
		return pice1.indexY == pice2.indexY;
	}

	private bool SameCol(Pice pice1, Pice pice2)
	{
		return pice1.indexX == pice2.indexX;
	}

	public void CheckNewLink()
	{
		for(int i = 0; i < board.xCount; i++)
		{
			for(int j = 0; j < board.yCount; j++)
			{
				var pice = board.GetPiceFromData(i, j);
				if(pice != null)
				{
					{
						var upper = board.GetPiceFromData(i, j+1);
						if(upper != null)
						{
							if(SameCol(pice, upper) && upper.indexY == pice.indexY + 1)
							{
								Linker.TryLink(pice, LinkDirectory.Top, upper);
							}
						}
					}
					{
						var downer = board.GetPiceFromData(i, j-1);
						if(downer != null)
						{
							if(SameCol(pice, downer) && downer.indexY == pice.indexY - 1)
							{
								Linker.TryLink(pice, LinkDirectory.Bottom, downer);
							}
						}
					}
					{
						var lefter = board.GetPiceFromData(i-1, j);
						if(lefter != null)
						{
							if(SameRow(pice, lefter) && lefter.indexX == pice.indexX - 1)
							{
								Linker.TryLink(pice, LinkDirectory.Left, lefter);
							}
						}
					}
					{
						var rightter = board.GetPiceFromData(i+1, j);
						if(rightter != null)
						{
							if(SameRow(pice, rightter) && rightter.indexX == pice.indexX + 1)
							{
								Linker.TryLink(pice, LinkDirectory.Right, rightter);
							}
						}
					}
				}
				
			}
		}
	}

	Dictionary<Pice, bool> tempDic = new Dictionary<Pice, bool>();
	public void CheckNewFix()
	{
		tempDic.Clear();
		for(int i = 0; i < board.xCount; i++)
		{
			for(int j = 0; j < board.yCount; j++)
			{
				var pice = board.GetPiceFromData(i, j);
				// 如果棋盘上这个位置没有 pice，则不处理
				if(pice == null)
				{
					continue;
				}
				// 如果这个 pice 已经检查过了，则不处理
				if(tempDic.ContainsKey(pice) &&  tempDic[pice])
				{
					continue;
				}
				var hasLeft = false;
				var hasRight = false;
				var hasBottom = false;
				var hasTop = false;
				
				pice.ForeachLinkedPiceIncludeSelf(linkedPice=>{
					// 设置已检查标志
					tempDic[pice] = true;
					// 如果在 board 上被放置到了正确的位置
					if(linkedPice.indexX == linkedPice.boardX && linkedPice.indexY == linkedPice.boardY)
					{
						if(linkedPice.indexX == 0)
						{
							hasLeft = true;
						}
						if(linkedPice.indexY == 0)
						{
							hasBottom = true;
						}
						if(linkedPice.indexX == map.xCount - 1)
						{
							hasRight = true;
						}
						if(linkedPice.indexY == map.yCount - 1)
						{
							hasTop = true;
						}
					}
				});

				// 如果包含两个相邻边，则固定以上所有 pice
				if((hasLeft && hasBottom) || (hasBottom && hasRight) || (hasRight && hasTop) || (hasTop && hasLeft))
				{
					pice.ForeachLinkedPiceIncludeSelf(linkedPice=>{
						// 如果这个 pice 是新 fixed 的则闪烁
						if(!linkedPice.isFixed)
						{
							linkedPice.Flash();
						}
						linkedPice.isFixed = true;
						//Debug.Log("fix: " + linkedPice.indexX + ", " + linkedPice.indexY);

					});
				}
			}
		}
	}

	public bool IsAllPiceFixed()
	{
		for(int i = 0; i < board.xCount; i++)
		{
			for(int j = 0; j < board.yCount; j++)
			{
				var pice = board.GetPiceFromData(i, j);
				if(pice == null || !pice.isFixed)
				{
					return false;
				}
			}
		}
		return true;
	}
}
