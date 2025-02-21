using System;

using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

#if BEPINEX

using BepInEx;

namespace MeshViewer {
    [BepInPlugin("com.github.Kaden5480.poy-mesh-viewer", "MeshViewer", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public void Awake() {
            // General
            config._toggleKeybind = Config.Bind(
                "General", "toggleKeybind", defaultToggleKeybind.ToString(),
                "Keybind to toggle the UI"
            );
            config.showUIByDefault = Config.Bind(
                "General", "showUIByDefault", true,
                "Whether the UI should be shown by default"
            );

            // Custom color
            config.render.color.red = Config.Bind(
                "Color", "red", 0,
                "The red component of the custom color"
            );
            config.render.color.green = Config.Bind(
                "Color", "green", 255,
                "The green component of the custom color"
            );
            config.render.color.blue = Config.Bind(
                "Color", "blue", 0,
                "The blue component of the custom color"
            );

            // Render
            config.render.peakBoundaries = Config.Bind(
                "Render", "peakBoundaries", false,
                "Whether to render objects on the PeakBoundary layer"
            );
            config.render.eventTriggers = Config.Bind(
                "Render", "eventTriggers", false,
                "Whether to render event triggers"
            );
            config.render.timeAttack = Config.Bind(
                "Render", "timeAttack", false,
                "Whether to render time attack zones"
            );
            config.render.windMillWings = Config.Bind(
                "Render", "windMillWings", false,
                "Whether to show which objects are on the WindMillWings layer"
            );
            config.render.windSectors = Config.Bind(
                "Render", "windSectors", false,
                "Whether to render wind sectors on Solemn Tempest"
            );

            // Colliders
            config.render.colliders.boxColliders = Config.Bind(
                "Colliders", "boxColliders", false,
                "Whether to forcibly render colliders"
            );
            config.render.colliders.capsuleColliders = Config.Bind(
                "Colliders", "capsuleColliders", false,
                "Whether to forcibly render colliders"
            );
            config.render.colliders.meshColliders = Config.Bind(
                "Colliders", "meshColliders", false,
                "Whether to forcibly render colliders"
            );
            config.render.colliders.sphereColliders = Config.Bind(
                "Colliders", "sphereColliders", false,
                "Whether to forcibly render colliders"
            );

            // Summit stuff
            config.render.summitStuff.opacity = Config.Bind(
                "SummitStuff", "opacity", 0.3f,
                "The opacity to render summit stuff with"
            );
            config.render.summitStuff.startRange = Config.Bind(
                "SummitStuff", "startRange", false,
                "Whether to render the range of the start location"
            );
            config.render.summitStuff.stamperRange = Config.Bind(
                "SummitStuff", "stamperRange", false,
                "Whether to render the range of the stamp box"
            );
            config.render.summitStuff.summitRange = Config.Bind(
                "SummitStuff", "summitRange", false,
                "Whether to render the range of the summit object"
            );
            config.render.summitStuff.summitLevel = Config.Bind(
                "SummitStuff", "summitLevel", false,
                "Whether to render the level of the summit object"
            );

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            CommonAwake();
        }

        public void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void OnGUI() {
            CommonGUI();
        }

        public void Update() {
            CommonUpdate();
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            CommonSceneLoad();
        }

        public void OnSceneUnloaded(Scene scene) {
            CommonSceneUnload();
        }

#elif MELONLOADER

using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(MeshViewer.Plugin), "MeshViewer", PluginInfo.PLUGIN_VERSION, "Kaden5480")]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace MeshViewer {
    public class Plugin: MelonMod {
        public override void OnInitializeMelon() {
            string filePath = $"{MelonEnvironment.UserDataDirectory}/com.github.Kaden5480.poy-mesh-viewer.cfg";

            // General
            MelonPreferences_Category general = MelonPreferences.CreateCategory("MeshViewer_General");
            general.SetFilePath(filePath);

            config._toggleKeybind = general.CreateEntry<string>("toggleKeybind", defaultToggleKeybind.ToString());
            config.showUIByDefault = general.CreateEntry<bool>("showUIByDefault", true);

            // Custom color
            MelonPreferences_Category color = MelonPreferences.CreateCategory("MeshViewer_Color");
            color.SetFilePath(filePath);

            config.render.color.red = color.CreateEntry<int>("red", 0);
            config.render.color.green = color.CreateEntry<int>("green", 255);
            config.render.color.blue = color.CreateEntry<int>("blue", 0);

            // Render
            MelonPreferences_Category render = MelonPreferences.CreateCategory("MeshViewer_Render");
            render.SetFilePath(filePath);

            config.render.peakBoundaries = render.CreateEntry<bool>("peakBoundaries", false);
            config.render.eventTriggers = render.CreateEntry<bool>("eventTriggers", false);
            config.render.windMillWings = render.CreateEntry<bool>("windMillWings", false);
            config.render.timeAttack = render.CreateEntry<bool>("timeAttack", false);
            config.render.windSectors = render.CreateEntry<bool>("windSectors", false);

            // Colliders
            MelonPreferences_Category colliders = MelonPreferences.CreateCategory("MeshViewer_Colliders");
            colliders.SetFilePath(filePath);

            config.render.colliders.boxColliders = render.CreateEntry<bool>("boxColliders", false);
            config.render.colliders.capsuleColliders = render.CreateEntry<bool>("capsuleColliders", false);
            config.render.colliders.meshColliders = render.CreateEntry<bool>("meshColliders", false);
            config.render.colliders.sphereColliders = render.CreateEntry<bool>("sphereColliders", false);

            // Summit stuff
            MelonPreferences_Category summitStuff = MelonPreferences.CreateCategory("MeshViewer_SummitStuff");
            summitStuff.SetFilePath(filePath);

            config.render.summitStuff.opacity = summitStuff.CreateEntry<float>("opacity", 0.6f);
            config.render.summitStuff.startRange = summitStuff.CreateEntry<bool>("startRange", false);
            config.render.summitStuff.stamperRange = summitStuff.CreateEntry<bool>("stamperRange", false);
            config.render.summitStuff.summitRange = summitStuff.CreateEntry<bool>("summitRange", false);
            config.render.summitStuff.summitLevel = summitStuff.CreateEntry<bool>("summitLevel", false);

            CommonAwake();
        }

        public override void OnGUI() {
            CommonGUI();
        }

        public override void OnUpdate() {
            CommonUpdate();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            CommonSceneLoad();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName) {
            CommonSceneUnload();
        }

#endif
        private KeyCode defaultToggleKeybind = KeyCode.Home;

        private Config.Cfg config = new Config.Cfg();
        private Cache cache;
        private UI ui;

        private void CommonAwake() {
            cache = new Cache(config);
            ui = new UI(cache);
        }

        private void CommonUpdate() {
            ui.Update();
        }

        private void CommonGUI() {
            ui.Render();
        }

        public void CommonSceneLoad() {
            cache.CacheObjects();
            cache.Update();
        }

        public void CommonSceneUnload() {
            cache.Clear();
        }
    }
}
