using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Puzzle 
{
	public event Action Complete;
	public event Action LoadComplete;

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
				instance.Init();
			}
			return instance;
		}
	}

	private Core core;
	private Map map;

	public static bool DEBUG = false;
	public static bool SHUFF = true;

	/// <summary>
	/// 整个游戏中只会被调用一次
	/// </summary>
	public void Init () 
	{
		instance = this;
		PiceManager.Init();
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

		this.board = core.board;
		this.side = core.side;
		core.Init();

	}

	public void ShowEye(bool b)
	{
		core.validSpriet.gameObject.SetActive(b);
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

	/// <summary>
	/// 每当开始新拼图时调用
	/// </summary>
	/// <param name="texture"></param>
	/// <param name="cellPixelSize"></param>
	public void StartPuzzle(Texture2D texture, int cellPixelSize)
	{
		Clean();
		EXPAND = (int)(cellPixelSize * 0.25f);
		expanedTexture = ExpandTexture(texture);
		map = new Map();
		map.Init(expanedTexture, EXPAND, cellPixelSize);

		board.Init(map.validWidth, map.validHeight, map.xCount, map.yCount);
		side.Init(200);
		//side.Init(map.validHeight, map.yCount);
		var count = map.xCount * map.yCount;

		var indexList = new List<int>();
		for(int i = 0; i < count; i++)
		{
			indexList.Add(i);
		}
		if(SHUFF)
		{
			indexList = ArraryUtil.GetRandomList(indexList);
		}
		foreach(var index in indexList)
		{
			var pice = PiceManager.Create(map, index);
			side.Append(pice);
		}
		side.RepositionPiceList();

		if(!DEBUG)
		{
			core.HideDot();
		}
		// board 的 scrollRect content位置归零
		var p = side.scrollView.content.localPosition;
		p.x = 0;
		side.scrollView.content.localPosition = p;
		// 设置 valid rect sprite 到参考图
		core.validSpriet.sprite = map.validSprite;
		core.validSpriet.gameObject.SetActive(false);
	}

	public void LoadInfo(PuzzleInfo info)
	{
		// 把所有 pice 拿起
		foreach(var pice in PiceManager.list)
		{
			pice.SetToFloating();
			pice.StopTween();
		}

		// 逐个设置 pice 归属
		foreach(var i in info.piceInfoList)
		{
			var index = i.index;
			var pice = PiceManager.GetByIndex(index);
			if(i.owner == PiceOwner.Board)
			{
				pice.SetToBoard(i.boardX, i.boardY);
			}
			else if(i.owner == PiceOwner.Side)
			{
				//pice.SetToSide(i.sideIndex);
				side.Append(pice);
			}
			else if(i.owner == PiceOwner.Floating)
			{
				pice.SetToSide(side.count - 1);
			}
			pice.isFixed = i.isFixed;
			pice.SortingOrder = i.sortingOrder;
			// linking
			pice.linkingList.Clear();
			foreach(var linkingInfo in i.LinkingInfoList)
			{
				var linking = new Linking();
				linking.directory = linkingInfo.directory;
				var linkingToPinceIndex = linkingInfo.piceIndex;
				var linkingToPice = PiceManager.GetByIndex(linkingToPinceIndex);
				linking.pice = linkingToPice;
				pice.linkingList.Add(linking);
			}
			// edge type
			pice.LeftType = i.leftType;
			pice.RightType = i.rightType;
			pice.BottomType = i.bottomType;
			pice.TopType = i.topType;
		}
		board.RepositionAllPiceNoAnimation();
		side.RepositionPiceListNoAnimation();

	}

	public void Clean()
	{
		PiceManager.Clean();
		LayerOrderDispatcher.Clean();
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
				var rect = side.GetCellRect(pice.sideIndex);
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

	public Pice lastDragPice;
	public void OnRootPiceDragEnd(Pice pice)
	{
		lastDragPice = pice;
		UpdatePiceOwner(pice);
		// 没有连结的 pice， 当拖放释放点不在 board 上时，自动回到 side 上
		if(pice.linkingList.Count == 0)
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
			CoroutineManager.Create(PlayCompleteTask());
		}
		
	}

	private IEnumerator PlayCompleteTask()
	{
		var temp = DicPool.Take();
		var rank = new List<List<Pice>>();
		var r1 = new List<Pice>();
		r1.Add(lastDragPice);
		rank.Add(r1);
		temp[lastDragPice] = true;

		var next = new List<Pice>();
		var last = r1;
		here:
		foreach(var pice in last)
		{
			foreach(var linking in pice.linkingList)
			{
				var nextPice = linking.pice;
				if(temp.ContainsKey(nextPice) && temp[nextPice])
				{
					continue;
				}
				next.Add(nextPice);
				temp[nextPice] = true;
			}
		}
		if(next.Count >0)
		{
			rank.Add(next);
			last = next;
			next = new List<Pice>();
			goto here;
		}
		DicPool.Put(temp);

		AudioManager.PlaySe("level-complete-wave");
		foreach(var list in rank)
		{
			foreach(var pice in list)
			{
				pice.Flash();
			}
			yield return new WaitForSeconds(0.15f);
		}
		yield return new WaitForSeconds(0.15f);

		AudioManager.PlaySe("level-complete");
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
				if(DEBUG)
				{
					Debug.Log("pice (" + pice.boardX + ", " + pice.boardY + ")");
				}
				// 如果这个 pice 已经检查过了，则不处理
				if(tempDic.ContainsKey(pice) && tempDic[pice])
				{
					if(DEBUG)
					{
						Debug.Log("already checked");
					}
					continue;
				}
				var hasLeft = false;
				var hasRight = false;
				var hasBottom = false;
				var hasTop = false;

				if(DEBUG)
				{
					Debug.Log("foreach linked pice");
				}
				pice.ForeachPiceOfBlock(linkedPice=>{
					if(DEBUG)
					{
						Debug.Log("linkedPice (" + linkedPice.boardX + ", " + linkedPice.boardY + ")");
					}
					
					// 设置已检查标志
					tempDic[linkedPice] = true;
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
					if(DEBUG)
					{
						Debug.Log("fix all");
					}
					pice.ForeachPiceOfBlock(linkedPice=>{
						if(DEBUG)
						{
							Debug.Log("fix (" + linkedPice.boardX + ", " + linkedPice.boardY + ")");
						}
						// 如果这个 pice 是新 fixed 的则闪烁
						if(!linkedPice.isFixed)
						{
							linkedPice.FlashAsFix();
						}
						linkedPice.isFixed = true;
						//Debug.Log("fix: " + linkedPice.indexX + ", " + linkedPice.indexY);

					});
				}
				else
				{

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

	public PuzzleInfo CreateInfo()
	{
		var info = new PuzzleInfo();
		foreach(var pice in PiceManager.list)
		{
			var piceInfo = pice.CreateInfo();
			info.piceInfoList.Add(piceInfo);
		}
		return info;
	}


}
