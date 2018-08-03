using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admission_PopdownOldPage : Admission
{
    Page oldPage;
    public override void Play(Page oldPage, Page newPage)
    {
        oldPage.Active = true;
        this.oldPage = oldPage;
        iTween.ScaleTo(oldPage.gameObject, iTween.Hash("scale", Vector3.zero, "easeType", "easeInBack", "time", 0.2f));
        this.lostTime = 0;
    }

    float lostTime = 0;
    public override void Update()
    {
        lostTime += Time.deltaTime;
        if(lostTime >= 0.2f)
        {
            oldPage.transform.localScale = Vector3.one;
            oldPage.Active = false;
            this.finished = true;
        }
    }
}
