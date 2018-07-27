using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Linking
{
	public Pice pice;
	public LinkDirectory directory;

	public LinkingInfo CreateInfo()
	{
		return new LinkingInfo
		{
			piceIndex = this.pice.index,
			directory = this.directory,
		};
	}
}

public enum LinkDirectory
{
	Left,
	Right,
	Top,
	Bottom,
}

public class LinkDirectoryUtil
{
	public static LinkDirectory Reverse(LinkDirectory directory)
	{
		switch(directory)
		{
			case LinkDirectory.Left:
				return LinkDirectory.Right;
			case LinkDirectory.Right:
				return LinkDirectory.Left;
			case LinkDirectory.Top:
				return LinkDirectory.Bottom;
			case LinkDirectory.Bottom:
				return LinkDirectory.Top;
			default:
				throw new Exception("unsupport directory: " + directory);
		}
	}
}