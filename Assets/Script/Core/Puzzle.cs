using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle 
{
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


	public void Init () {
		instance = this;
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
		var map = new Map();
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

	void Link(Pice pice1, LinkDirectory directory, Pice pice2)
	{
		// set pice1
		{
			var info = new LinkInfo();
			info.directory = directory;
			info.pice = pice2;
			pice1.linking.Add(info);
		}
		// set pice2
		{
			var info = new LinkInfo();
			info.directory = LinkDirectoryUtil.Reverse(directory);
			info.pice = pice1;
			pice2.linking.Add(info);
		}
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
		if(pice.linking.Count == 0)
		{
			var inboard = board.Rect.Contains(pice.transform.position);
			if(inboard)
			{
				board.PlacePice(pice);
				CheckNewLink();
			}
			else
			{
				side.PlacePice(pice);
			}
		}
		else
		{
			board.PlacePice(pice);
			CheckNewLink();
		}
	}

	void TryLink(Pice pice1, LinkDirectory directory, Pice pice2)
	{
		if(pice1.owner != PiceOwner.Board || pice2.owner != PiceOwner.Board)
		{
			return;
		}
		var alreadyLinked = false;
		pice1.linking.ForEach(info =>{
			if(info.directory == directory)
			{
				alreadyLinked = true;
			}
		});
		if(alreadyLinked)
		{
			return;
		}
		Link(pice1, directory, pice2);
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
								TryLink(pice, LinkDirectory.Top, upper);
							}
						}
					}
					{
						var downer = board.GetPiceFromData(i, j-1);
						if(downer != null)
						{
							if(SameCol(pice, downer) && downer.indexY == pice.indexY - 1)
							{
								TryLink(pice, LinkDirectory.Bottom, downer);
							}
						}
					}
					{
						var lefter = board.GetPiceFromData(i-1, j);
						if(lefter != null)
						{
							if(SameRow(pice, lefter) && lefter.indexX == pice.indexX - 1)
							{
								TryLink(pice, LinkDirectory.Left, lefter);
							}
						}
					}
					{
						var rightter = board.GetPiceFromData(i+1, j);
						if(rightter != null)
						{
							if(SameRow(pice, rightter) && rightter.indexX == pice.indexX + 1)
							{
								TryLink(pice, LinkDirectory.Right, rightter);
							}
						}
					}
				}
				
			}
		}
	}

}
