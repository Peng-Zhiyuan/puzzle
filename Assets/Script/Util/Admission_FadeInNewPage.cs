using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admission_OldDownNewUp : Admission 
{
	public override void Play(Page oldPage, Page newPage)
	{
		CoroutineManager.Create(PlayCR(oldPage, newPage));
	}

	IEnumerator PlayCR(Page oldPage, Page newPage)
	{
		oldPage.Active = true;
		newPage.Active = false;
		iTween.MoveBy(oldPage.gameObject, new Vector2(0, -2000), 0.5f);
		yield return new WaitForSeconds(0.3f);
		newPage.transform.localPosition = new Vector2(0, -2000);
		newPage.Active = true;
		oldPage.Active = false;
		iTween.MoveBy(newPage.gameObject, new Vector2(0, 2000), 0.5f);
		yield return new WaitForSeconds(0.3f);
		this.finished = true;
	}

}
