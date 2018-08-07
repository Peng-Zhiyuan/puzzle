using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeadBarFloating : Floating 
{
	GameObject button_back;

	public HeadBar_GoldItem goldItem;
	public HeadBar_Star starItem;
	public HeadBar_Calendar calendarItem;
	public HeadBar_Mail mailItem;
	public HeadBar_Like likeItem;

	public RectTransform backButton;

	public int Gold
	{
		set
		{
			this.goldItem.Value = value.ToString();
		}
		get
		{
			return int.Parse(this.goldItem.Value);
		}
	}

	public int Star
	{
		set
		{
			this.starItem.value = value.ToString();
		}
		get
		{
			return int.Parse(this.starItem.value);
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
			if(top is LevelCompletePage)
			{
				// do nothing
			}
			else if(top is DisplayPage)
			{
				// do nothing
			}
			else if(top is DialogPage)
			{
				// do nothing
				var dialog = top as DialogPage;
				dialog.OnCloseButton();
			}
			else if(top is AdPage)
			{
				// do nothing
				var adPage = top as AdPage;
				adPage.OnCloseButton();
			}
			if(top is LevelSettingsPage)
			{
				var admin = new Admission_PopdownOldPage();
				UIEngine.Back(null, admin);
			}
			else if(top is ShopPage)
			{
				var admin = new Admission_OldDownNewUp();
				UIEngine.Back(null, admin);
			}
			else if(top is CorePage)
			{
				var popup = new Admission_PopupNewPage();
				var dialog = UIEngine.Forward<DialogPage>("退出会存储已进行的拼图，确定要退出吗？", popup);
				dialog.Complete = DialogResult =>
				{
					if(DialogResult == DialogResult.Conform)
					{
						GameController.SaveUncompletePuzzle();
						//UIEngine.BackTo<PicturePage>();
						CoroutineManager.Create(WaitAndReturn());
					}
				};
			}
			else
			{
				UIEngine.Back(null, admission);
			}
			AudioManager.PlaySe("button");
			SDKManager.OnHeadBarBackbutton();
		}
		else
		{
			Debug.Log("this is the only one page, can't call UIEngine.Back()");
		}
	}

	public IEnumerator WaitAndReturn()
	{
		yield return new WaitForSeconds(0.3f);
		UIEngine.BackTo<PicturePage>();
		Puzzle.Instance.Clean();
	}

	public static HeadBarFloating instance;
	public static Admission admission;

	public override void OnCreate()
	{
		instance = this;
		AutoRefresh = true;
	}

	Page lastPage;
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(AutoRefresh)
		{
			this.goldItem.Value = PlayerStatus.gold.ToString();
			this.starItem.value = PlayerStatus.exp.ToString();
			this.starItem.Process = PlayerStatus.LevelUpProcess;
		}

		var nowTop = UIEngine.Top;
		if(nowTop != lastPage)
		{
			if(nowTop is MainPage || nowTop is LevelCompletePage || nowTop is DisplayPage)
			{
				HideBack();
				Debug.Log("HideBack");
			}
			else
			{
				ShowBack();
				Debug.Log("ShowBack");
			}
			lastPage = nowTop;
		}
	}

	public void OnGoldClicked()
	{
		var top = UIEngine.Top;
		if(top is CorePage)
		{
			return;
		}
		if(top is ShopPage)
		{
			this.OnBackButton();
			return;
		}
		UIEngine.ShowFlaoting("BackgroundFloating");
		var admission = new Admission_OldDownNewUp();
        UIEngine.Forward<ShopPage>(null, admission);
		AudioManager.PlaySe("button");
		AudioManager.PlaySe("sign-and-shop");
	}

	public void OnStarClicked()
	{
		UIEngine.ShowFloating<StarFloating>();
		AudioManager.PlaySe("button");
	}

	public void OnSignClicked()
	{
		var admin = new Admission_PopupNewPage();
		UIEngine.Forward<SignPage>(null, admin);
		AudioManager.PlaySe("button");
		AudioManager.PlaySe("sign-and-shop");
	}

	public void HideBack()
	{
		iTween.Stop(backButton.gameObject);
		var wp = backButton.position;
		wp.x = - 110;
		iTween.MoveTo(backButton.gameObject, wp, 0.2f);
	}

	public void ShowBack()
	{
		iTween.Stop(backButton.gameObject);
		var wp = backButton.position;
		wp.x = 0;
		iTween.MoveTo(backButton.gameObject, wp, 0.2f);
	}

	public Rect GoldWolrdRect
	{
		get
		{
			return goldItem.WorldRect();
		}
	}

	public Rect ExpWolrdRect
	{
		get
		{
			return starItem.WorldRect();
		}
	}

	private bool _autoRefresh;
	public bool AutoRefresh
	{
		get
		{
			return _autoRefresh;
		}
		set
		{
			_autoRefresh = value;
		}
	}

	public void ScaleGold()
	{
		CoroutineManager.Create(_ScaleGoldTask());
	}

	private IEnumerator _ScaleGoldTask()
	{
		var rt = goldItem.text.GetComponent<RectTransform>();
		rt.DOScale(2, 0.2f);
		yield return new WaitForSeconds(0.2f);
		rt.DOScale(1, 0.2f);
	}

	public void ScaleExp()
	{
		CoroutineManager.Create(_ScaleExpTask());
	}

	private IEnumerator _ScaleExpTask()
	{
		var rt = starItem.text.GetComponent<RectTransform>();
		rt.DOScale(2, 0.2f);
		yield return new WaitForSeconds(0.2f);
		rt.DOScale(1, 0.2f);
	}
}
