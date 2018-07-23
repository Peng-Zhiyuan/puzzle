using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Admission_ScaleDownOldPage: Admission
{
    Rect rect;
    float DURATION = 0.2f;

    public Admission_ScaleDownOldPage(Rect rect)
    {
        this.rect = rect;
    } 

    public override void Play(Page oldPage, Page newPage)
    {
        oldPage.Active = true;
        newPage.Active = true;
        // newPage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, rect.xMin, rect.size.x);
        // newPage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, rect.yMin,rect.size.y);
        
        // tween center
        var fromPosition = oldPage.transform.position;
        var targetPosition = this.rect.center;
        // oldPage.transform.position = this.rect.center;
        oldPage.rectTransform.DOMove(targetPosition, DURATION);

        // tween scale
        var canvas = UIEngine.Canvas.GetComponent<RectTransform>();
        var canvasWidth = canvas.rect.width;
        var canvasHeight = canvas.rect.height;
        var fromScaleX = this.rect.width / canvasWidth;
        var fromScaleY = this.rect.height / canvasHeight;
        // newPage.rectTransform.localScale = Vector2.one; 
        oldPage.rectTransform.DOScale(new Vector2(fromScaleX, fromScaleY), DURATION);

        // tween alpha
        var cg = TryGetComponent<CanvasGroup>(oldPage.gameObject);
        cg.alpha = 1;
        cg.DOFade(0, DURATION).OnComplete(()=>{
            this.finished = true;
            oldPage.Active = false;
            cg.alpha = 1;
        });

    }

    static T TryGetComponent<T>(GameObject a) where T: Component
    {
        var c = a.GetComponent<T>();
        if(c != null)
        {
            return c;
        }
        return a.AddComponent<T>();
    }
}
