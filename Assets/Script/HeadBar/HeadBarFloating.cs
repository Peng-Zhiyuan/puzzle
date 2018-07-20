using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBarFloating : Floating 
{
	GameObject button_back;

	public HeadBar_GoldItem goldItem;
	public HeadBar_Star starItem;
	public HeadBar_Calendar calendarItem;
	public HeadBar_Mail mailItem;
	public HeadBar_Like likeItem;

	public int Gold
	{
		set
		{
			this.goldItem.Value = value.ToString();
		}
	}

	public int Star
	{
		set
		{
			this.starItem.value = value.ToString();
		}
	}

	public int Days
	{
		set
		{
			this.calendarItem.Value = calendarItem.ToString();
		}
	}

	public bool IsMailShowNotification
	{
		set
		{
			this.mailItem.IsShowNotification = value;
		}
	}

	public int LikeAddGoldCount
	{
		set
		{
			this.likeItem.Value = value.ToString();
		}
	}
}
