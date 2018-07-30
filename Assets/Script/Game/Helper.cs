using UnityEngine;
using System.Collections.Generic;
using CustomLitJson;
using System;

public static class Helper
{
    public static void AddGold(int value)
    {
        PlayerStatus.gold += value;
        AudioManager.PlaySe("gain-gold");
    }
}