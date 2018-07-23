using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admission_PopupNewPage : Admission
{
    public override void Play(Page oldPage, Page newPage)
    {
        newPage.transform.localScale = Vector3.zero;
        iTween.ScaleTo(newPage.gameObject, iTween.Hash("scale", Vector3.one, "easeType", "easeOutBack", "time", 0.2f));
        this.lostTime = 0;
    }

    float lostTime = 0;
    public override void Update()
    {
        lostTime += Time.deltaTime;
        if(lostTime >= 0.2f)
        {
            this.finished = true;
        }
    }
}
