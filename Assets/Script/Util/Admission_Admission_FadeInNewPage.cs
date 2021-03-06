﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Admission_FadeInNewPage : Admission 
{
	public override void Play(Page oldPage, Page newPage)
	{
		//CoroutineManager.Create(PlayCR(oldPage, newPage));
		//iTween.FadeFrom(newPage.gameObject, 0f, 0.2f);
		var cg = GameObjectUtil.TryGetComponent<CanvasGroup>(newPage.gameObject);
        cg.alpha = 0.2f;
        cg.DOFade(1, 0.2f).OnComplete(()=>{
            this.finished = true;
            oldPage.Active = false;
        });
	}

	// float lossTime;
	// public override void Update()
	// {
	// 	lossTime += Time.deltaTime;
	// 	if(lossTime >= 0.2f)
	// 	{
	// 		this.finished = true;
	// 	}
	// }

	// IEnumerator PlayCR(Page oldPage, Page newPage)
	// {
	// 	oldPage.Active = true;
	// 	newPage.Active = false;
	// 	iTween.MoveBy(oldPage.gameObject, new Vector2(0, -2000), 0.5f);
	// 	yield return new WaitForSeconds(0.3f);
	// 	newPage.transform.localPosition = new Vector2(0, -2000);
	// 	newPage.Active = true;
	// 	oldPage.Active = false;
	// 	iTween.MoveBy(newPage.gameObject, new Vector2(0, 2000), 0.5f);
	// 	yield return new WaitForSeconds(0.3f);
	// 	this.finished = true;
	// }

}
