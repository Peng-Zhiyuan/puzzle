using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using NativeBuilder;
using UnityEngine.Assertions;

public class Build 
{

    /// <summary>
    /// 命令行统一编译方法(的CLI接口)
    /// </summary>
    /// 
    public static void UniversalBuild()
    {
        // 命令行参数检查
        var args = System.Environment.GetCommandLineArgs ();

        // 默认参数
        var backup = "";

        // 处理命令行选项
        ReadOptions((option, arg) =>
            {
                switch(option)
                {
                case "-backup":
                    backup = arg;
                    break;
                    // 在这里处理更多的命令行选项...
                }
            });

        var platform = EditorUserBuildSettings.activeBuildTarget;
        switch (platform)
        {
            case BuildTarget.Android:
                {
                    var task = new BuildTask_Android(BuildLevel.JustAndroidProject, backup, "");
                    task.Build();
                    break;
                }
            case BuildTarget.iOS:
                {
                    var task = new BuildTask_iOS(IOSBuildLevel.JustXCodeProject, backup);
                    task.Build();
                    break;
                }
                // 在这里添加更多的编译平台...

        }
    }

    private static void ReadOptions(Action<string, string> onOption)
    {
        var args = System.Environment.GetCommandLineArgs ();
        for (int i = 0; i < args.Length; i++) {
            if (args [i].StartsWith("-", StringComparison.Ordinal)) {
                string option = args[i];
                string arg = "";
                if (i + 1 < args.Length)
                {
                    if (!args[i + 1].StartsWith("-", StringComparison.Ordinal))
                    {
                        arg = args[i + 1];
                    }
                }
                onOption(option, arg);
            }
        }
    }


}

