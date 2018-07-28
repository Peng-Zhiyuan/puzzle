using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingPage : Page 
{
	public Image pice;

    public override void OnPush()
    {
        CoroutineManager.Create(PreloadingTask());
    }


    private IEnumerator PreloadingTask()
    {
        var c = pice.color;
        c.a = 0;
        pice.color = c;
        pice.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        var resList = Resources.LoadAll("ui-engine");
        Debug.Log(resList.Length + " res loaded");
        pice.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        UIEngine.Replace("MainPage");
    }
}
