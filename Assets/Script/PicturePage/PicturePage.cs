using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicturePage : Page
{
    public Text text_title;
    public Text text_des;

    public override void OnParamChanged()
    {
        var pictype = this.param;
        var name = StaticDataLite.GetCell<string>("pictype", pictype, "display_name");
        var des = StaticDataLite.GetCell<string>("pictype", pictype, "des");
        this.text_title.text = name;
        this.text_des.text = des;
    }
}
