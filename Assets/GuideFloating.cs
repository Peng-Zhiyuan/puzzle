using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GuideFloating : Floating 
{
	public Image bgblack;
	public Image hand;
	public RectTransform copyRoot_nonclick;
	public RectTransform copyRoot;
	public static bool guideMode;
	public Text des;
	public Button fullButton;
	public Button clickMask;

	protected override void OnActive()
	{
		guideMode = true;
		StartCoroutine(Do());
	}

	public IEnumerator Do()
	{
		clickMask.gameObject.SetActive(true);
		if(!PlayerStatus.IsTodaySigned())
		{
			yield return StepAsync("SignPage", "content/button_get", "SIGN", 184, MsgList.Get("guide1"));
		}
		yield return StepAsync("MainPage", "scrollRect/content/image_grid/animal", "SELECT_PIC_TYPE", 740, MsgList.Get("guide2"));
		yield return StepAsync("PicturePage", "scrollRect/content/1", "SELECT_PIC", 700, MsgList.Get("guide3"));
		yield return StepAsync("LevelSettingsPage", "content/button_newGame", "NEW_GAME", 150, MsgList.Get("guide4"));

		yield return StepClickAsync("CorePage", 500, MsgList.Get("guide5"));
		guideMode = false;
		PlayerStatus.needGide = false;
		PlayerStatus.Save();
		clickMask.gameObject.SetActive(false);

		yield return CorePageHandGuide();
		UIEngine.HideFlaoting("GuideFloating");
	}

	public IEnumerator StepAsync(string pageName, string goPath, string waiteMsg, float desY, string des)
	{
		// 等待签到页面出现
		yield return WaiteTopPage(pageName);

		// 等待动画完成
		yield return new WaitForSeconds(0.2f);

		// 复制节点到无法点击root
		var topPage = UIEngine.Top;
		var button_get = FindChild(topPage.transform, goPath).GetComponent<RectTransform>();
		var copy = GameObject.Instantiate(button_get);
		copy.parent = this.copyRoot_nonclick;
		//copy.anchoredPosition = button_get.anchoredPosition;
		copy.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, button_get.sizeDelta.x);
		copy.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, button_get.sizeDelta.y);
		copy.position = button_get.position;
		copy.localScale = button_get.localScale;

		// 等待显示黑幕
		yield return this.FadeInBlackAsync();

		// 等待显示手
		StartCoroutine(this.FadeInDesAsync(des, desY));
		yield return this.FadeInHandlAsync(copy.position);

		// 移动复制对象到可点击root
		copy.parent = this.copyRoot;

		// 等待签到事件
		yield return RadioStation.WaiteMsgAsync(waiteMsg);

		// 摧毁复制
		GameObject.Destroy(copy.gameObject);

		// 等待隐藏手和黑幕
		StartCoroutine(this.HideDesAsync());
		yield return this.HideHandAsync();
		yield return this.HideBlackAsync();
	}

	public IEnumerator StepClickAsync(string pageName, float desY, string des)
	{
		// 等待签到页面出现
		yield return WaiteTopPage(pageName);

		// 等待动画完成
		yield return new WaitForSeconds(0.2f);

		// 等待显示黑幕
		StartCoroutine(this.FadeInDesAsync(des, desY));
		yield return this.FadeInBlackAsync();

		// 等点点击屏幕
		yield return WaiteClickFullButtonAsync();
		
		// 等待隐藏手和黑幕
		StartCoroutine(this.HideDesAsync());
		yield return this.HideBlackAsync();
	}

	public IEnumerator WaiteTopPage(string pageName)
	{
		var top = UIEngine.Top;
		while((top.name != pageName))
		{
			yield return null;
			top = UIEngine.Top;
		}
	}



	public IEnumerator FadeInBlackAsync()
	{
		this.bgblack.gameObject.SetActive(true);
		var c = this.bgblack.color;
		c.a = 0;
		this.bgblack.color = c;
		var time = 0.2f;
		this.bgblack.DOFade(0.65f, time);
		yield return new WaitForSeconds(time);
	}

	public IEnumerator FadeInDesAsync(string des, float y)
	{
		this.des.text = des;
		this.des.gameObject.SetActive(true);
		var rt = this.des.GetComponent<RectTransform>();
		rt.anchoredPosition = new Vector2(0, y);
		var c = this.des.color;
		c.a = 0;
		this.des.color = c;
		var time = 0.2f;
		this.des.DOFade(0.5f, time);
		yield return new WaitForSeconds(time);
	}

	public IEnumerator FadeInHandlAsync(Vector2 position)
	{
		this.hand.gameObject.SetActive(true);
		var rt = this.hand.GetComponent<RectTransform>();
		rt.position = position;
		var c = this.hand.color;
		c.a = 0;
		this.hand.color = c;
		var time = 0.2f;
		this.hand.DOFade(0.5f, time);
		yield return new WaitForSeconds(time);
	}

	public IEnumerator HideHandAsync()
	{
		var time = 0.2f;
		this.hand.DOFade(0f, time);
		yield return new WaitForSeconds(time);
		this.hand.gameObject.SetActive(false);
	}

	public IEnumerator HideDesAsync()
	{
		var time = 0.2f;
		this.des.DOFade(0f, time);
		yield return new WaitForSeconds(time);
		this.des.gameObject.SetActive(false);
	}

	public IEnumerator HideBlackAsync()
	{
		var time = 0.2f;
		this.bgblack.DOFade(0f, time);
		yield return new WaitForSeconds(time);
		this.bgblack.gameObject.SetActive(false);
	}

	public static Transform FindChild(Transform obj, string path)
	{
		var parts = path.Split('/');
		for(var i = 0; i < parts.Length; i++)
		{
			var name = parts[i];
			obj = obj.transform.Find(name);
			if(obj == null)
			{
				return null;
			}
		}
		return obj;

	}

	bool clicked = false;
	public IEnumerator WaiteClickFullButtonAsync()
	{
		clicked = false;
		this.fullButton.gameObject.SetActive(true);
		while(!clicked)
		{
			yield return null;
		}
		this.fullButton.gameObject.SetActive(false);
	}

	public void OnFullButton()
	{
		clicked = true;
	}

	public IEnumerator CorePageHandGuide()
	{
		this.hand.gameObject.SetActive(true);
		var rt = this.hand.GetComponent<RectTransform>();
		var top = UIEngine.Top;
		if(top.name == "CorePage")
		{
			var piceLocation = FindChild(top.transform, "PiceLocation");
			this.hand.transform.position = piceLocation.position;
		}
		var c = this.hand.color;
		c.a = 0;
		this.hand.color = c;
		var time = 0.2f;
		this.hand.DOFade(0.5f, time);
		
		var startPosition = this.hand.transform.position;
		var targetY = this.hand.transform.position.y + 500;
		var count = 0;
		while(this.hand.gameObject.activeInHierarchy && UIEngine.Top.name == "CorePage")
		{
			this.hand.transform.position = startPosition;
			this.hand.transform.DOMoveY(targetY, 1f);
			yield return new WaitForSeconds(1f);
			count ++;
			if(count >= 5)
			{
				break;
			}
		}

		this.hand.DOFade(0f, time);
		yield return new WaitForSeconds(time);
	}
}
