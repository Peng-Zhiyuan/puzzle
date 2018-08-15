using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SignPage : Page 
{
	public SignPageSmallItem sample_smallItem;
	public SignPageBigItem smaple_bigItem;
	public Transform layout;
	public Button button_got;

	SignPageSmallItem selectItem1;
	SignPageBigItem selectItem2;
	public override void OnPush()
	{
		if(PlayerStatus.sign == 7)
		{
			if(!PlayerStatus.IsTodaySigned())
			{
				PlayerStatus.lastSignDay = 0;
				PlayerStatus.sign = 0;
			}
		}

		for(int i = 1; i <= 6; i++)
		{
			var item = GameObject.Instantiate(sample_smallItem);
			item.Title = GetTitleOfDay(i);
			item.value = GetGoldOfDay(i).ToString();
			item.IsGot = IsSigned(i);
			item.IsSelected = IsSelect(i);
			var anchor = FindLayoutAnchor(i);
			TransformUtil.DestroyAllChildren(anchor);
			item.transform.parent = anchor;
			item.transform.localPosition = Vector2.zero;
			item.transform.localScale = Vector2.one;
			anchor.GetComponent<Image>().enabled = false;
			item.gameObject.SetActive(true);
			if(IsSelect(i))
			{
				selectItem1 = item;
			}
		}
		{
			var item = GameObject.Instantiate(smaple_bigItem);
			item.Title = GetTitleOfDay(7);
			item.value = GetGoldOfDay(7).ToString();
			item.IsGot = IsSigned(7);
			item.IsSelected = IsSelect(7);
			var anchor = FindLayoutAnchor(7);
			TransformUtil.DestroyAllChildren(anchor);
			item.transform.parent = anchor;
			item.transform.localPosition = Vector2.zero;
			item.transform.localScale = Vector2.one;
			anchor.GetComponent<Image>().enabled = false;
			item.gameObject.SetActive(true);
			if(IsSelect(7))
			{
				selectItem2 = item;
			}
		}
		sample_smallItem.gameObject.SetActive(false);
		smaple_bigItem.gameObject.SetActive(false);

		// 今天是否已经领取过
		if(PlayerStatus.IsTodaySigned())
		{
			button_got.interactable = false;
			var text = button_got.GetComponentInChildren<Text>();
			text.text = "GOT";
			text.color = new Color(1f, 1f, 1f, 0.7f);
		}
		else
		{
			button_got.interactable = true;
			var text = button_got.GetComponentInChildren<Text>();
			text.text = "GET";
			text.color = new Color(1f, 1f, 1f, 1f);
		}
	}

	int GetGoldOfDay(int day)
	{
		return StaticDataLite.GetCell<int>("sign", day.ToString(), "gold");
	}

	string GetTitleOfDay(int day)
	{
		return StaticDataLite.GetCell<string>("sign", day.ToString(), "title");
	}

	Transform FindLayoutAnchor(int day)
	{
		var t = layout.Find(day.ToString());
		return t;
	}

	bool IsSelect(int day)
	{
		if(!PlayerStatus.IsTodaySigned())
		{
			if(PlayerStatus.sign + 1 == day)
			{
				return true;
			}
		}
		else
		{
			if(PlayerStatus.sign == day)
			{
				return true;
			}
		}

		return false;
	}

	int GetSelectedDay()
	{
		return PlayerStatus.sign + 1;
	}

	bool IsSigned(int day)
	{
		return day <= PlayerStatus.sign;
	}
	
	public void OnGotButton()
	{
		if(PlayerStatus.IsTodaySigned())
		{
			return;
		}
		var day = PlayerStatus.sign + 1;
		var gold = GetGoldOfDay(day);
		PlayerStatus.gold += gold;
		PlayerStatus.sign += 1;
		SetLastSignDayAsToday();
		PlayerStatus.Save();

		if(selectItem1 != null)
		{
			selectItem1.Flash();
			selectItem1.IsGot = true;
		}
		if(selectItem2 != null)
		{
			selectItem2.Flash();
			selectItem2.IsGot = true;
		}

		button_got.interactable = false;
		var text = button_got.GetComponentInChildren<Text>();
		text.text = "GOT";
		text.color = new Color(1f, 1f, 1f, 0.7f);
		AudioManager.PlaySe("gain-gold");
		CoroutineManager.Create(WaitAndBack());
		RadioStation.Brodcast("SIGN");
	}


    public IEnumerator WaitAndBack()
    {
        yield return new WaitForSeconds(0.35f);
        var admin = new Admission_PopdownOldPage();
        UIEngine.Back(null, admin);
    }

	public void OnClose()
	{
		var admin = new Admission_PopdownOldPage();
		UIEngine.Back(null, admin);
		AudioManager.PlaySe("button");
	}

	public void SetLastSignDayAsToday()
	{
		var today = DateTime.UtcNow.Day;
		PlayerStatus.lastSignDay = today;
	}


}
