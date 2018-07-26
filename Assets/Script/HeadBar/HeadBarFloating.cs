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

	public void OnBackButton()
	{
		if(UIEngine.PagesCount > 1)
		{
			var top = UIEngine.Top;
			if(top is LevelSettingsPage)
			{
				var admin = new Admission_PopdownOldPage();
				UIEngine.Back(null, admin);
			}
			else
			{
				UIEngine.Back(null, admission);
			}
		}
		else
		{
			Debug.Log("this is the only one page, can't call UIEngine.Back()");
		}
	}

	public static HeadBarFloating instance;
	public static Admission admission;

	public override void OnCreate()
	{
		instance = this;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		this.goldItem.Value = GameStorage.Gold.ToString();
		this.starItem.value = GameStorage.star.ToString();
	}
}
