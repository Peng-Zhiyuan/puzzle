using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour 
{

	public Board board;
	public Side side;

	public void Init()
	{
		var t = this.transform.Find("bg");
		t.localScale = new Vector2(Game.heightScale * 1.25f, Game.heightScale * 1.25f);
	}

	public void HideDot()
	{
//		var content = this.transform.Find("content");
//		var dot = content.Find("dot");
//		dot.gameObject.SetActive(false);
	}
}
