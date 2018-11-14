using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class UIUtil
{
    public static Task<DialogResult> DialogAsync(string msg, string buttonText)
    {
        var tcs = new TaskCompletionSource<DialogResult>();
        var param = new DialogParam();
        param.des = msg;
        param.button = buttonText;
        var popup = new Admission_PopupNewPage();
        var dialog = UIEngine.Forward<DialogPage>(param, popup);
        dialog.Complete = result =>{
            UIEngineHelper.WateAdmissionComplete(()=>{
                tcs.SetResult(result);
            });
        };
        return tcs.Task;
    }

    public static Task<bool> ShowADPageAsync(AdPageOpenSources surce)
    {
        var tcs = new TaskCompletionSource<bool>();
        AdPage.sources = surce;
        var admin = new Admission_PopupNewPage();
        var adPage = UIEngine.Forward<AdPage>(null, admin);
        adPage.Compelte = () => {
            UIEngineHelper.WateAdmissionComplete(()=>{
                tcs.SetResult(true);
            });
        };
        return tcs.Task;
    }
}