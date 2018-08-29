using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TimestampUtil
{
    /// <summary>
    /// 生成时间戳 
    /// </summary>
    /// <returns>当前时间减去 1970-01-01 00.00.00 得到的毫秒数</returns>
    public static long Now
    {
        get
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            long unixTime = (long)System.Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

    }
}