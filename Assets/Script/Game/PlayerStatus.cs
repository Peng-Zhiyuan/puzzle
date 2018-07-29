using UnityEngine;
using System.Collections.Generic;
using CustomLitJson;
using System;

public static class PlayerStatus
{
    public static int exp;
    public static int gold;
    public static int sign;
    public static int lastSignDay;

    public static Dictionary<string, CoreInfo> uncompletePuzzle = new Dictionary<string, CoreInfo>();
    public static Dictionary<string, CompleteInfo> completeDic;

    public static void Save()
    {
        // misc data
        PlayerPrefs.SetInt("PlayerStatus.gold", gold);
        PlayerPrefs.SetInt("PlayerStatus.exp", exp);
        PlayerPrefs.SetInt("PlayerStatus.sign", sign);
        PlayerPrefs.SetInt("PlayerStatus.lastSignDay", lastSignDay);
        // uncomplete
        {
            var json = JsonMapper.Instance.ToJson(uncompletePuzzle);
            PlayerPrefs.SetString("PlayerStatus." + nameof(uncompletePuzzle), json);
        }

        // complete list
        {
            var json = JsonMapper.Instance.ToJson(completeDic);
            PlayerPrefs.SetString("PlayerStatus." + nameof(completeDic), json);
        }

        // flush
        PlayerPrefs.Save();
    }
    

    public static void Read()
    {
        // misc data
        exp = PlayerPrefs.GetInt("PlayerStatus.exp", 0);
        gold = PlayerPrefs.GetInt("PlayerStatus.gold", 0);
        sign =  PlayerPrefs.GetInt("PlayerStatus.sign", 0);
        lastSignDay = PlayerPrefs.GetInt("PlayerStatus.lastSignDay", 0);
        // uncomplete
        {
            var json = PlayerPrefs.GetString("PlayerStatus." + nameof(uncompletePuzzle), "{}");
            uncompletePuzzle = JsonMapper.Instance.ToObject<Dictionary<string, CoreInfo>>(json);
        }
        // complete list
        {
            var json = PlayerPrefs.GetString("PlayerStatus." + nameof(completeDic), "{}");
            completeDic = JsonMapper.Instance.ToObject<Dictionary<string, CompleteInfo>>(json);
        }
    }


    public static int Level
    {
        get
        {
            var sheet = StaticDataLite.GetSheet("level");
            var level = 0;
            foreach(string id in sheet.Keys)
            {
                var row = sheet[id];
                var needExp = row.Get<int>("needexp");
                if(exp >= needExp)
                {
                    level = int.Parse(id);
                }
                else
                {
                    break;
                }
            }
            return level;
        }
    }

    public static int NeedExpToNextLevel
    {
        get
        {
            var level = Level;
            var nextLevel = level + 1;
            if(StaticDataLite.GetRow("level", nextLevel.ToString()) != null)
            {
                var totalExp = StaticDataLite.GetCell<int>("level", nextLevel.ToString(), "needexp");
                var lackExp = totalExp - exp;
                return lackExp;
            }
            else
            {
                return -1;
            }
        }
    }

    public static float LevelUpProcess
    {
        get
        {
            var level = Level;
            var nextLevel = level + 1;
            if(StaticDataLite.GetRow("level", nextLevel.ToString()) == null)
            {
                return -1;
            }
            var a = StaticDataLite.GetCell<int>("level", level.ToString(), "needexp");
            var b = StaticDataLite.GetCell<int>("level", nextLevel.ToString(), "needexp");
            float total = b - a;
            float current = exp - a;
            return current / total;
        }
    }

    public static CoreInfo FirstUncompletePuzzleInfo
    {
        get
        {
            foreach(var kv in uncompletePuzzle)
            {
                return kv.Value;
            }
            return null;
        }
    }

    public static void RemoveUncompleteInfoOfPicId(int picId)
    {
        uncompletePuzzle.Remove(picId.ToString());
    }

    public static CompleteInfo GetCompleteInfoOfPicId(int picId)
    {
        CompleteInfo info;
        completeDic.TryGetValue(picId.ToString(), out info);
        return info;
    }

    public static bool IsPictureComplete(int picId)
    {
        var info = GetCompleteInfoOfPicId(picId);
        return info != null;
    }

    public static CoreInfo TryGetUncompleteOfPicId(int picId)
    {
        CoreInfo info;
        uncompletePuzzle.TryGetValue(picId.ToString(), out info);
        return info;
    }
}