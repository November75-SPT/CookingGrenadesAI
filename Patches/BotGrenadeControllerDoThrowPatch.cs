using System;
using System.Reflection;
using SPT.Reflection.Patching;
using EFT.UI.Map;
using HarmonyLib;
using EFT.UI.WeaponModding;
using UnityEngine;
using System.Collections.Generic;
using CookingGrenadesAI.Config;
using System.Threading.Tasks;
using EFT;
using SPT.Custom.Utils;
using EFT.InputSystem;
using AnimationEventSystem;
using Comfort.Common;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CookingGrenadesAI.Patches;


public class QuickGrenadeThrowHandsControllerSpawnPatch : ModulePatch
{
    // Stores the cooking wait time for each player to synchronize grenade throw timing
    public static Dictionary<WeakReference<Player>, float> CookingTimeMap = new Dictionary<WeakReference<Player>, float>(new WeakReferenceComparer());
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.DeclaredMethod(typeof(Player.QuickGrenadeThrowHandsController), nameof(Player.QuickGrenadeThrowHandsController.Spawn));
    }
    [PatchPostfix]
    public static void PatchPostfix(Player.QuickGrenadeThrowHandsController __instance, Player ____player)
    {
        Player player = ____player;
        BotGrenadeController botGrenadeController = player.AIData.BotOwner.WeaponManager.Grenades;
        ThrowWeapItemClass grenade = botGrenadeController.grenade;
        Vector3 target = botGrenadeController.AIGreanageThrowData.Target;
        if (!player.IsAI)
        {
            return;
        }
        if (IsDebugLogConfigOn())
        {
            string grenadeName = new MongoID(grenade.TemplateId).LocalizedShortName();
            float explDelay = grenade.GetExplDelay;
            string logText = $"{player.name}({player.Profile.Nickname}, LVL {player.Profile.Info.Level}) " +
                $"{grenadeName} (ExplDelay: {explDelay:F4}s)";
            LogFileAndIngame(logText);
        }
        var level = player.Profile.Info.Level;
        if (!ShouldCookGrenade(level))
        {
            return;
        }      
        

        float flightTime = CalculateFlightTime(__instance,player ,target);                

        float cookingWaitTime = botGrenadeController.grenade.GetExplDelay - flightTime;
        
        cookingWaitTime = AdjustErrorCookingTime(cookingWaitTime, level);


        // if wait time is over the grenade.GetExplDelay(cook fail self explosive)
        float clampCookingWaitTime = Mathf.Clamp(cookingWaitTime, 0f, botGrenadeController.grenade.GetExplDelay);
        if (IsDebugLogConfigOn())
        {
            if (clampCookingWaitTime != cookingWaitTime)
            {
                string logText = $"Adjust between 0~GrenadeFuzeTime({grenade.GetExplDelay}): {cookingWaitTime}=>{clampCookingWaitTime}";
                LogFileAndIngame(logText);
            }
        }
        if (clampCookingWaitTime <= 0f)
        {
            if (IsDebugLogConfigOn())
            {
                string logText = $"Cooking aborted: Wait time {clampCookingWaitTime} is below then 0";
                LogFileAndIngame(logText);
            }
            return;
        }

        // Pause animation
        __instance.FirearmsAnimator.Animator.speed = 0;
        // ___gparam_0.FirearmsAnimator.Animator.enabled = false;
        // Calculate wait time including throw animation (avg 0.7842s)
        const float avgThrowAnimationTime = 0.7842f;
        float adjustedWaitTime = clampCookingWaitTime - avgThrowAnimationTime;
        __instance.WaitSeconds(adjustedWaitTime, () => { __instance.FirearmsAnimator.Animator.speed = 1f; });

        var playerRef = new WeakReference<Player>(player);
        CookingTimeMap.Add(playerRef, clampCookingWaitTime);

        if (IsDebugLogConfigOn())
        {
            string logText = $"Cook Success {clampCookingWaitTime}s";
            LogFileAndIngame(logText);
        }


        /* 
        Note: Average time from here to grenade spawn (BaseGrenadeHandsController.vmethod_2) is ~0.7842s
            0.7860001
            0.7949999
            0.784
            0.7940001
            0.7880001
            0.7819999
            0.784
            0.7920001
            0.783
            0.784
            0.786
            0.783
            0.785
            0.783
        */
    }
    // Calculates cooking chance and returns if AI should cook grenade
    public static bool ShouldCookGrenade(int level)
    {
        var levelTable = Singleton<BackendConfigSettingsClass>.Instance.Experience.Level.Table;
        var maxLevel = levelTable.Length;

        float minLevelCookingChance = ConfigManager.MinLevelCookingChance.Value;
        float maxLevelCookingChance = ConfigManager.MaxLevelCookingChance.Value;

        // Calculate cooking chance using linear interpolation
        float chance = Mathf.Lerp(minLevelCookingChance, maxLevelCookingChance, (float)(level - 1) / (maxLevel - 1));
        var rand = UnityEngine.Random.value;
        if (IsDebugLogConfigOn())
        {
            string logText = $"LVL {level,2}: Chance {chance:F4}, Random {rand:F4}";
            LogFileAndIngame(logText);
        }
        return rand < chance;        
    }
    // Calculates grenade flight time based on throw trajectory
    // Get the logic that computes when creating a grenade and precompute it.
    static float CalculateFlightTime(Player.QuickGrenadeThrowHandsController controller, Player player, Vector3 target)
    {
        Vector3 throwPosition = controller.FindThrowPosition();

        // Calculate throw direction and force (based on BaseGrenadeHandsController.vmethod_1)
        Vector3 direction = Vector3.up;
        float lowHighThrow = 1f;
        float forcePower = 3f;

        BotGrenadeController grenades = player.AIData.BotOwner.WeaponManager.Grenades;
        float num3 = ((grenades.Mass <= 0.01f) ? 0.5f : grenades.Mass);
        forcePower = grenades.AIGreanageThrowData.Force * num3;
        direction = NormalizeFastSelf(grenades.ToThrowDirection);


        // Compute throw force (based on BaseGrenadeHandsController.method_9)
        Vector3 force = direction * (forcePower * lowHighThrow);

#if DEBUG
        // Create debug marker at target position
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.position = target;
        marker.transform.localScale = Vector3.one * 0.3f;
        marker.GetComponent<Renderer>().material.color = Color.green;
        UnityEngine.Object.Destroy(marker.GetComponent<Collider>());
        UnityEngine.Object.Destroy(marker, 15f);
#endif

        // Estimate flight time - made by GROK AI
        float mass = 0.5f;
        Vector3 velocity = force / mass; // Initial velocity
        float distance = Vector3.Distance(throwPosition, target); // Distance to target
        float speed = velocity.magnitude; // Speed magnitude
        float estimatedTime = speed > 0 ? distance / speed : 1f; // Time = distance / speed

        float curveFactor = 1f + 0.125f * distance / 10f; // Add 12.5% per 10m for curve
        return estimatedTime * curveFactor;
    }
    // Adjusts cooking time with level-based error
    public static float AdjustErrorCookingTime(float baseWaitTime, int level)
    {
        // Adjust time accuracy based on AI level
        // Interpolates time error
        float minErrorLowLevel = ConfigManager.MinErrorLowLevel.Value;
        float maxErrorLowLevel = ConfigManager.MaxErrorLowLevel.Value;
        float minErrorHighLevel = ConfigManager.MinErrorHighLevel.Value;
        float maxErrorHighLevel = ConfigManager.MaxErrorHighLevel.Value;

        var levelTable = Singleton<BackendConfigSettingsClass>.Instance.Experience.Level.Table;
        int maxLevel = levelTable.Length;
                    
        float minErrorCurrent = Mathf.Lerp(minErrorLowLevel, minErrorHighLevel, (float)(level - 1) / (maxLevel - 1));
        float maxErrorCurrent = Mathf.Lerp(maxErrorLowLevel, maxErrorHighLevel, (float)(level - 1) / (maxLevel - 1));
        float timeAccuracyFactor = UnityEngine.Random.Range(minErrorCurrent, maxErrorCurrent);
        float throwTimeOffset = baseWaitTime * timeAccuracyFactor;

        // Randomly speeds up or slows down throw timing
        if (UnityEngine.Random.value > 0.5f)
        {
            throwTimeOffset *= -1f;
        }

        if (IsDebugLogConfigOn())
        {
            // string logText = $"ErrorRange {minErrorCurrent:F3}~{maxErrorCurrent:F3}=>{timeAccuracyFactor:F3}\n"+
            //                 $"WaitTime({baseWaitTime:F3})+offset({throwTimeOffset:F3})={(baseWaitTime+throwTimeOffset):F3}";
            string logText = string.Format(
                "LVL {0,2} | Error Range: {1:F4} ~ {2:F4} => {3:F4} | Base: {4:F4} + Offset: {5:F4} | Result: {6:F4}",
            level, minErrorCurrent, maxErrorCurrent, timeAccuracyFactor, baseWaitTime, throwTimeOffset, baseWaitTime + throwTimeOffset);
            LogFileAndIngame(logText);
        }

        return baseWaitTime + throwTimeOffset;
    }
    static bool IsDebugLogConfigOn()
    {
        return ConfigManager.LogToFile.Value || ConfigManager.LogToDebugWindow.Value;
    }
    static void LogFileAndIngame(string message)
    {
        if (ConfigManager.LogToFile.Value)
        {
            Plugin.log.LogInfo(message);
        }
        if (ConfigManager.LogToDebugWindow.Value)
        {
            Utils.DebugDisplay.Instance.InsertFixedDisplayObject(message);
        }
    }

    // Normalizes a vector (from GClass834.NormalizeFastSelf)
    public static Vector3 NormalizeFastSelf(Vector3 v)
    {
        float num = (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        v.x /= num;
        v.y /= num;
        v.z /= num;
        return v;
    }
}


public class QuickGrenadeThrowHandsControllerClass1162OnDropGrenadeActionPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.DeclaredMethod(typeof(Player.QuickGrenadeThrowHandsController.Class1162), nameof(Player.QuickGrenadeThrowHandsController.Class1162.OnDropGrenadeAction));
    }
    [PatchPrefix]
    public static void PatchPrefix(Player.QuickGrenadeThrowHandsController.Class1162 __instance, ref float ___float_0, Player.BaseGrenadeHandsController ___gparam_0)
    {
        Player player = Traverse.Create(___gparam_0).Field("_player").GetValue<Player>();
        var cookingMap = QuickGrenadeThrowHandsControllerSpawnPatch.CookingTimeMap;

        // Apply stored cooking time and remove from map
        
        var playerRef = new WeakReference<Player>(player);
        
        if (cookingMap.Remove(playerRef, out float cookingTime))
        {
            ___float_0 = cookingTime;
        }
    }
}
public class WeakReferenceComparer : IEqualityComparer<WeakReference<Player>>
{
    public bool Equals(WeakReference<Player> x, WeakReference<Player> y)
    {
        return x.TryGetTarget(out var xTarget) && y.TryGetTarget(out var yTarget) && xTarget == yTarget;
    }

    public int GetHashCode(WeakReference<Player> obj)
    {
        return obj.TryGetTarget(out var target) ? target.GetHashCode() : 0;
    }
}