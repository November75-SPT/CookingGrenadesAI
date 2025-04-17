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

namespace CookingGrenadesAI.Patches;

// bot recheck time throw grenade 
public class BotGrenadeControllermethod_7Patch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(BotGrenadeController), nameof(BotGrenadeController.method_7));
        
    }
    [PatchPostfix]
    public static void PatchPostfix(BotGrenadeController __instance)
    {
        BotGrenadeController controller = __instance;
        controller._nextPosibleAttempt = Time.time + 0.5f;
        string debugText =  $"_nextPosibleAttempt,{controller.botOwner_0.name}: {controller._nextPosibleAttempt}";
        // Plugin.log.LogInfo(debugText);
    }
}