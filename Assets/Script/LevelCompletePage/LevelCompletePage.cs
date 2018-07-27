using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePage : Page 
{
	public Text label_exp;
	public Text label_gold;

	public RectTransform goldGroup;
	public RectTransform expGroup;

	public static int goldParam;
	public static int expParam;


	public override void OnPush()
	{
		this.label_exp.text = expParam.ToString();
		this.label_gold.text = goldParam.ToString();

		goldGroup.gameObject.SetActive(true);
		expGroup.gameObject.SetActive(true);

		{
			var light = this.transform.Find("light");
			iTween.Stop(light.gameObject);
			iTween.RotateBy(light.gameObject, iTween.Hash("amount", new Vector3(0, 0, 1), "time", 200, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
		}

		{
			var right = this.transform.Find("ring_right");
			var light = right.Find("light");
			iTween.Stop(light.gameObject);
			iTween.RotateBy(light.gameObject, iTween.Hash("amount", new Vector3(0, 0, 1), "time", 10, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
		}

		{
			var left = this.transform.Find("ring_left");
			var light = left.Find("light");
			iTween.Stop(light.gameObject);
			iTween.RotateBy(light.gameObject, iTween.Hash("amount", new Vector3(0, 0, 1), "time", 10, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
		}

	}

	private IEnumerator TaskGold()
	{
		Debug.Log("TaskGold");
		//yield return new WaitForSeconds(2f);
		var clone = GameObject.Instantiate(goldGroup);
		clone.parent = goldGroup.parent;
		clone.localPosition = goldGroup.localPosition;
		iTween.Stop(clone.gameObject);
		goldGroup.gameObject.SetActive(false);
		iTween.ScaleTo(clone.gameObject, new Vector2(0.2f, 0.2f), 0.2f);
		yield return new WaitForSeconds(0.2f);

		var headbar = UIEngine.GetComtrol<HeadBarFloating>();
		var rect = headbar.GoldWolrdRect;
		var p = rect.center;

		iTween.MoveTo(clone.gameObject, p, 0.2f);
		yield return new WaitForSeconds(0.2f);

		HeadBarFloating.instance.Gold += goldParam;
		HeadBarFloating.instance.ScaleGold();
	}

	private IEnumerator TaskExp()
	{
		Debug.Log("TaskExp");
		//yield return new WaitForSeconds(2f);
		var clone = GameObject.Instantiate(expGroup);
		clone.parent = expGroup.parent;
		clone.localPosition = expGroup.localPosition;
		iTween.Stop(clone.gameObject);
		expGroup.gameObject.SetActive(false);
		iTween.ScaleTo(clone.gameObject, new Vector2(0.2f, 0.2f), 0.2f);
		yield return new WaitForSeconds(0.2f);

		var headbar = UIEngine.GetComtrol<HeadBarFloating>();
		var rect = headbar.ExpWolrdRect;
		var p = rect.center;

		iTween.MoveTo(clone.gameObject, p, 0.2f);
		yield return new WaitForSeconds(0.2f);

		HeadBarFloating.instance.Star += expParam;
		HeadBarFloating.instance.ScaleExp();
	}

	private IEnumerator Task()
	{
		StartCoroutine(TaskExp());
		yield return new WaitForSeconds(0.2f);
		StartCoroutine(TaskGold());
		yield return new WaitForSeconds(0.5f);
		HeadBarFloating.instance.AutoRefresh = true;
		UIEngine.BackTo<PicturePage>();
		UIEngine.DestroyFromPool("LevelCompletePage");
	}

	public void OnFullButton()
	{
		CoroutineManager.Create(Task());
	}
}
