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
        {
            var resList = Resources.LoadAll("ui-engine");
            Debug.Log("ui-engine: " + resList.Length + " res loaded");
        }
        {
            var resList = Resources.LoadAll("audio-manager");
            Debug.Log("audio-manager: " + resList.Length + " res loaded");
        }
        pice.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
        UIEngine.Replace("MainPage");

        var index = 0;
        if(PlayerStatus.bgmIndex == 0)
        {
            index = 2;//UnityEngine.Random.Range(1, 3);
        }
        else
        {
            var lastIndex = PlayerStatus.bgmIndex;
            if(lastIndex == 1)
            {
                index = 2;
            }
            else
            {
                index = 1;
            }
        }
        PlayerStatus.bgmIndex = index;
        PlayerStatus.Save();
        var bgm = "bgm" + index;
        Debug.Log("bgm: " + bgm);
        AudioManager.PlayBgm(bgm);

    }
}
