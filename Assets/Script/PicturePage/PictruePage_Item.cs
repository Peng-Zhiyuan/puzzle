using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PictruePage_Item : MonoBehaviour 
{
	public PictruePage_ItemData data;
	public Image picture;
	public Image puzzleMask;
	public Transform unlockLayer;
	public Text label;
	public Image flash;
	public Image image_pice;
	public Text label_unlockButotn;

	public Sprite Sprite
	{
		set
		{
			this.picture.sprite = value;
		}
	}

	public bool IsShowPuzzleMask
	{
		set
		{
			this.puzzleMask.gameObject.SetActive(value);
		}
	}

	public bool IsShowUnlockLayer
	{
		set
		{
			this.unlockLayer.gameObject.SetActive(value);
		}
	}

	public string LabelText
	{
		set
		{
			this.label.text = value;
		}
	}

	public bool IsShowPice
	{
		set
		{
			this.image_pice.gameObject.SetActive(value);
		}
	}

	public int PiceCount
	{
		set
		{
			var text = this.image_pice.GetComponentInChildren<Text>();
			text.text = value.ToString();
		}
	}

	public string UnlockButtonText
	{
		set
		{
			label_unlockButotn.text = value;
		}
	}

	public void OnUnlockButton()
	{
		SendMessageUpwards("OnItemUnlockButton", this);
	}

	public void OnClicked()
	{
		SendMessageUpwards("OnItemClicked", this);
	}

	public void Flash()
	{
		flash.gameObject.SetActive(true);
		var image =  flash.GetComponent<Image>();
		var c = image.color;
		c.a = 1f;
		image.color = c;
		flash.DOFade(0, 0.5f);
	}
}
