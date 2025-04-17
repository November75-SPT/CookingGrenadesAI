namespace CookingGrenadesAI.Config;
public static class ConfigEventHandler
{
    public static void Init()
    {
        
        var CookingChanceTester = ConfigManager.EnableCookingChanceTest;
        CookingChanceTester.SettingChanged += (sender, args) =>
        {
            if (CookingChanceTester.Value)
            {
                CookingChanceTester.Value = false; // Run once and reset
                Utils.ConfigTester.RunCookingChanceTest();
            }
        };

        var EnableErrorTest = ConfigManager.EnableErrorTest;
        EnableErrorTest.SettingChanged += (sender, args) =>
        {
            if (EnableErrorTest.Value)
            {
                EnableErrorTest.Value = false; // Run once and reset
                Utils.ConfigTester.RunErrorTest();
            }
        };
    }
}
