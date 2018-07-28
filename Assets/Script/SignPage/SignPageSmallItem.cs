using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SignPageSmallItem : MonoBehaviour 
{
	public Sprite sprite_unselect;
	public Sprite sprite_select;

	public Image image_bg;
	public Image image_get;
	public Text text_title;
	public Text text_value;
	public Image flash;

	public bool IsSelected
	{
		set
		{
			if(value)
			{
				image_bg.sprite = sprite_select;
			}
			else
			{
				image_bg.sprite = sprite_unselect;
			}
		}
	}

	public string Title
	{
		set
		{
			this.text_title.text = value;
		}
	}

	public bool IsGot
	{
		set
		{
			this.image_get.gameObject.SetActive(value);
		}
	}

	public string value
	{
		set
		{
			this.text_value.text = value;
		}
	}

	public void Flash()
	{
		flash.gameObject.SetActive(true);
		flash.enabled = true;
		var c = flash.color;
		c.a = 1;
		flash.color = c;
		flash.DOFade(0, 0.5f);
	}

}
