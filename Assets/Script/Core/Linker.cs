using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Linker
{
	public static void TryLink(Pice pice1, LinkDirectory directory, Pice pice2)
	{
		// 如果任一 pice 不在 board 上，则不予处理
		if(pice1.owner != PiceOwner.Board || pice2.owner != PiceOwner.Board)
		{
			return;
		}

		// 如果 pice1 的指定方向上已经连结，则不予处理
		var alreadyLinked = false;
		pice1.linkingList.ForEach(info =>{
			if(info.directory == directory)
			{
				alreadyLinked = true;
			}
		});
		if(alreadyLinked)
		{
			return;
		}

		// 连接两个 pice
		Link(pice1, directory, pice2);
	}

    public static void Link(Pice pice1, LinkDirectory directory, Pice pice2)
	{
		// set pice1
		{
			var info = new Linking();
			info.directory = directory;
			info.pice = pice2;
			pice1.linkingList.Add(info);
			if(pice1.linkingList.Count == 1)
			{
				pice1.FlashAsLink();
			}
			
		}
		// set pice2
		{
			var info = new Linking();
			info.directory = LinkDirectoryUtil.Reverse(directory);
			info.pice = pice1;
			pice2.linkingList.Add(info);
			if(pice2.linkingList.Count == 1)
			{
				pice2.FlashAsLink();
			}
		}
	}
}