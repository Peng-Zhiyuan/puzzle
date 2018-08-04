using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomLitJson;

public static class PushManager
{
    public static void ResetNotification()
    {
        LocalNotification.ClearNotifications();
        var now = DateTime.Now;
        var sheet = StaticDataLite.GetSheet("push");
        var dataList = new List<PushData>();
        foreach(string key in sheet.Keys)
        {
            var row = sheet[key];
            var data = new PushData();
            data.row = row;
            var h = row.Get<int>("h");
            var m = row.Get<int>("m");
            var s = row.Get<int>("s");
            data.shike = new DateTime(now.Year, now.Month, now.Day, h, m, s);
            dataList.Add(data);
        }

        for(int i = 0; i < 5; i++)
        {
            foreach(var data in dataList)
            {
                data.shike = data.shike.AddDays(1);
                var delay = data.shike - now;
                var title = data.row.Get<string>("title");
                var text = data.row.Get<string>("text");
                LocalNotification.SendNotification(delay, title, text, new Color32(0xff, 0x44, 0x44, 255));
                Debug.Log("set notification at： " + data.shike);
            }
        }
    }

    class PushData
    {
        public DateTime shike;
        public JsonData row;
    }
}

