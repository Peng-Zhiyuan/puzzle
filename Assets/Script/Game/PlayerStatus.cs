using UnityEngine;
using System.Collections.Generic;
using CustomLitJson;

public static class PlayerStatus
{
    public static int exp;
    public static int gold;
    public static Dictionary<string, CoreInfo> uncompletePuzzle = new Dictionary<string, CoreInfo>();

    public static void Save()
    {
        PlayerPrefs.SetInt("PlayerStatus.gold", gold);
        PlayerPrefs.SetInt("PlayerStatus.exp", exp);
        var json = JsonMapper.Instance.ToJson(uncompletePuzzle);
        Debug.Log(json);
        PlayerPrefs.SetString("PlayerStatus.uncompletePuzzle", json);
        PlayerPrefs.Save();
    }

    public static void Read()
    {
        exp = PlayerPrefs.GetInt("PlayerStatus.exp", 0);
        gold = PlayerPrefs.GetInt("PlayerStatus.gold", 0);
        var json = PlayerPrefs.GetString("PlayerStatus.uncompletePuzzle", "{}");
        Debug.Log(json);
        uncompletePuzzle = JsonMapper.Instance.ToObject<Dictionary<string, CoreInfo>>(json);
        Debug.Log("obj:" + JsonMapper.Instance.ToJson(uncompletePuzzle));
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

    public static CoreInfo TryGetUncompleteOfPicId(int picId)
    {
        CoreInfo info;
        uncompletePuzzle.TryGetValue(picId.ToString(), out info);
        return info;
    }
}