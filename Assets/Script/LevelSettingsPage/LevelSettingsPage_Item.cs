using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomLitJson;

public class LevelSettingsPage_Item : MonoBehaviour 
{
	public JsonData dataRow;
	public Text text;

	public void SetBig()
	{
		text.color =  new Color(219/255f, 198/255f, 23/255f, 255/255f);
		text.fontSize = 120;
		text.resizeTextMaxSize = 120;
		// iTween.Stop(this.gameObject);
		// var color = new Color(219/255f, 198/255f, 23/255f, 255/255f);
		// var fontSize = 120;
		// iTween.ColorTo(this.text.gameObject, color, 0.2f);
		// var font = text.fontSize;
		// iTween.ValueTo(this.gameObject, iTween.Hash("from", font, "to", fontSize, "onupdate", "Tween", "time", 0.2f));
	}

	private void Tween(float value)
	{
		text.fontSize = (int)value;
	}

	public void SetSmall()
	{
		text.color = new Color(135/255f, 83/255f, 38/255f, 255/255f);
		text.fontSize = 72;
		text.resizeTextMaxSize = 72;
	}

}
