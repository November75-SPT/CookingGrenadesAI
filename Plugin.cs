using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using CookingGrenadesAI.Config;
using CookingGrenadesAI.Patches;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Linq;

namespace CookingGrenadesAI
{
    [BepInPlugin("com.November75.CookingGrenadesAI", "CookingGrenadesAI", BuildInfo.Version)]
    [BepInDependency("com.SPT.core", "3.11.0")]
    [BepInDependency("com.November75.NoFixedCookGrenadesAI", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource log;
        private void Awake()
        {
            log = Logger;
            ConfigManager.Init(Config);
            ConfigEventHandler.Init();

            // If anyone wants to read my code, I apologize for that. I was writing like crazy and it got messed up😭.
            new QuickGrenadeThrowHandsControllerSpawnPatch().Enable();
            new QuickGrenadeThrowHandsControllerClass1162OnDropGrenadeActionPatch().Enable();

#if DEBUG
            // make bot keep throw
            new BotGrenadeControllermethod_7Patch().Enable();
            /*
            to change scav grenade carry amount is
            \SPT_Data\Server\database\bots\types\assault.json
                generation.items.grenades.weights
            */ 
#endif
        }
    }
}

