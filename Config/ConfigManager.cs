using BepInEx.Configuration;

namespace CookingGrenadesAI.Config;
internal static class ConfigManager
{
    public static ConfigEntry<float> MinLevelCookingChance;
    public static ConfigEntry<float> MaxLevelCookingChance;
    #region TimeError
    public static ConfigEntry<float> MinErrorLowLevel;
    public static ConfigEntry<float> MaxErrorLowLevel;
    public static ConfigEntry<float> MinErrorHighLevel;
    public static ConfigEntry<float> MaxErrorHighLevel;
    #endregion TimeError

    #region Tester
    public static ConfigEntry<int> TestLevel;
    public static ConfigEntry<float> TestBaseTime;
    public static ConfigEntry<bool> EnableCookingChanceTest;
    public static ConfigEntry<bool> EnableErrorTest;
    #endregion Tester
    #region Debug
    public static ConfigEntry<bool> LogToFile;
    public static ConfigEntry<bool> LogToDebugWindow;
    #endregion Debug

    public static void Init(ConfigFile configFile)
    {
        MinLevelCookingChance = configFile.Bind(
            "1. Cooking Chance",
             "Min Level Cooking Chance",
            0.2f,
            new ConfigDescription(
                "Cooking chance for level 1 AI (0.2 is 20% chance)",
                new AcceptableValueRange<float>(0f, 1f),
                new ConfigurationManagerAttributes { Order = 10, ShowRangeAsPercent = false }));

        MaxLevelCookingChance = configFile.Bind(
            "1. Cooking Chance",
            "Max Level Cooking Chance",
            0.5f,
            new ConfigDescription(
                "Cooking chance for max-level AI (0.5 is 50% chance)",
                new AcceptableValueRange<float>(0f, 1f),
                new ConfigurationManagerAttributes { Order = 9, ShowRangeAsPercent = false }));



        #region TimeError
        MinErrorLowLevel = configFile.Bind(
            "2. Wait Time Error",
            "Min Error at Low Level",
            0.3f,
            new ConfigDescription(
                "Minimum timing error at level 1 AI (fraction of wait time)",
                new AcceptableValueRange<float>(0f, 1f),
                new ConfigurationManagerAttributes { Order = 10, ShowRangeAsPercent = false }));

        MaxErrorLowLevel = configFile.Bind(
            "2. Wait Time Error",
            "Max Error at Low Level",
            0.5f,
            new ConfigDescription(
                "Maximum timing error at level 1 AI (fraction of wait time)",
                new AcceptableValueRange<float>(0f, 1f),
                new ConfigurationManagerAttributes { Order = 9, ShowRangeAsPercent = false }));

        MinErrorHighLevel = configFile.Bind(
            "2. Wait Time Error",
            "Min Error at High Level",
            0.0f,
            new ConfigDescription(
                "Minimum timing error at max-level AI (fraction of wait time)",
                new AcceptableValueRange<float>(0f, 1f),
                new ConfigurationManagerAttributes { Order = 8, ShowRangeAsPercent = false }));

        MaxErrorHighLevel = configFile.Bind(
            "2. Wait Time Error",
            "Max Error at High Level",
            0.1f,
            new ConfigDescription(
                "Maximum timing error at max-level AI (fraction of wait time)",
                new AcceptableValueRange<float>(0f, 1f),
                new ConfigurationManagerAttributes { Order = 7, ShowRangeAsPercent = false }));

        #endregion TimeError



        #region Tester
        TestLevel = configFile.Bind(
            "3. Tester",
            "Test Level",
            0,
            new ConfigDescription(
                "Level to test (0 tests all levels)",
                new AcceptableValueRange<int>(0, 80),
                new ConfigurationManagerAttributes { Order = 11 }));

        TestBaseTime = configFile.Bind(
            "3. Tester",
            "Test Base Time",
            0f,
            new ConfigDescription(
                "Base time to test (0 tests 0.5 to 5.0 step 0.5)",
                new AcceptableValueRange<float>(0f, 10f),
                new ConfigurationManagerAttributes { Order = 10 }));
        EnableCookingChanceTest = configFile.Bind(
            "3. Tester", 
            "Cooking Chance Tester", 
            false, 
            new ConfigDescription(
                "",
                null,
                new ConfigurationManagerAttributes {Order = 9 }));
        EnableErrorTest = configFile.Bind(
            "3. Tester",
            "Error Tester",
            false,
            new ConfigDescription(
                "",
                null,
                new ConfigurationManagerAttributes { Order = 8 }));
        #endregion Tester

        #region Debug

        LogToFile = configFile.Bind(
            "4. Debug", 
            "Log to file", 
            false, 
            new ConfigDescription(
                "File is BepInEx/LogOutput.log",
                null,
                new ConfigurationManagerAttributes { Order = 10 }));
        LogToDebugWindow = configFile.Bind(
            "4. Debug",
            "Log to Ingame Debug Window",
            false,
            new ConfigDescription(
                "",
                null,
                new ConfigurationManagerAttributes { Order = 9 }));
        #endregion Debug
    }
}
