using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarFloating : Floating 
{
	public Text label_level;
	public Text label_needStar;
	public Slider slider;

	public RectTransform desgroup;
	public RectTransform content;

	protected override void OnActive()
	{
		this.label_level.text = PlayerStatus.Level.ToString();
		var lackexp = PlayerStatus.NeedExpToNextLevel;
		if(lackexp != -1)
		{
			desgroup.gameObject.SetActive(true);
			this.label_needStar.text = lackexp.ToString();
		}
		else
		{
			// hide
			desgroup.gameObject.SetActive(false);
		}
		var process = PlayerStatus.LevelUpProcess;
		Debug.Log("process: " + process);
		if(process != -1)
		{
			slider.value = process;
		}
		else
		{
			// hide
			slider.value = 1;
		}

		// tween content
		iTween.Stop(this.content.gameObject);
        this.content.transform.localScale = Vector3.zero;
        iTween.ScaleTo(this.content.gameObject, iTween.Hash("scale", Vector3.one, "easeType", "easeOutBack", "time", 0.2f));
	}

	public void OnFollButton()
	{
		UIEngine.HideFlaoting<StarFloating>();
	}
}
