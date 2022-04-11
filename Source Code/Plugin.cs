using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace SeventysToyKnfieMod
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }
        public static ConfigEntry<bool> isModEnabled;
        void Start()
        {
            ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "SeventysToyKnife.cfg"), true);
            isModEnabled = config.Bind<bool>("Config", "Is Mod Active?", true, "Toggle mod here");
        }
        void OnGameInitialized(object sender, EventArgs e)
        {
            if (isModEnabled.Value)
            {
                Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("SeventysToyKnfieMod.Assets.toyknfie");
                AssetBundle bundle = AssetBundle.LoadFromStream(str);
                if (bundle == null)
                {
                    Debug.Log("Failed to load AssetBundle!");
                    return;
                }
                var toyknife = bundle.LoadAsset<GameObject>("Toy Knife");

                GameObject lefthand = GameObject.Find("palm.01.R");


                Instantiate(toyknife);
                GameObject knifeInstance = GameObject.Find("Toy Knife(Clone)");
                knifeInstance.transform.SetParent(lefthand.transform, false);
                knifeInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                knifeInstance.transform.localPosition = new Vector3(-0.04f, 0.04f, - 0.05f);
                knifeInstance.transform.localEulerAngles = new Vector3(8.0831f, 263.5094f, 155.9368f);
            }
        }

        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
