using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using CookingGrenadesAI.Config;
using EFT.GlobalEvents;

namespace CookingGrenadesAI.Utils;

internal class ConfigTester
{
    private static readonly float[] DefaultTestTimes = { 0.5f, 1.0f, 1.5f, 2.0f, 2.5f, 3.0f, 3.5f, 4.0f, 4.5f, 5.0f };
    public static void RunCookingChanceTest()
    {
        var levelTable = Singleton<BackendConfigSettingsClass>.Instance.Experience.Level.Table;
        var maxLevel = levelTable.Length;
        int minLevel = 1;

        if (ConfigManager.TestLevel.Value != 0)
        {
            maxLevel = Config.ConfigManager.TestLevel.Value;
            minLevel = Config.ConfigManager.TestLevel.Value;
        }
        for (int level = minLevel; level <= maxLevel; level++)
        {
            Patches.QuickGrenadeThrowHandsControllerSpawnPatch.ShouldCookGrenade(level);
        }
    }
    public static void RunErrorTest()
    {
        var levelTable = Singleton<BackendConfigSettingsClass>.Instance.Experience.Level.Table;
        var maxLevel = levelTable.Length;
        int minLevel = 1;
        if (ConfigManager.TestLevel.Value != 0)
        {
            maxLevel = Config.ConfigManager.TestLevel.Value;
            minLevel = Config.ConfigManager.TestLevel.Value;
        }
        List<float> testTimes = ConfigManager.TestBaseTime.Value != 0f
            ? new List<float> { ConfigManager.TestBaseTime.Value }
            : DefaultTestTimes.ToList();        
        
        for (int level = minLevel; level <= maxLevel; level++)
        {
            testTimes.ForEach(time => Patches.QuickGrenadeThrowHandsControllerSpawnPatch.AdjustErrorCookingTime(time, level));
        }
    }
}