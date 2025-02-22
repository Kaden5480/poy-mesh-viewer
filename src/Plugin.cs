using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

#if BEPINEX

using BepInEx;

namespace MeshViewer {
    [BepInPlugin("com.github.Kaden5480.poy-mesh-viewer", "MeshViewer", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        /**
         * <summary>
         * Executes when the plugin is initialized.
         * </summary>
         */
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

            // Colors
            config.colors.peakBoundaries = Config.Bind(
                "Colors", "peakBoundaries", colorPeakBoundaries,
                "The color to use for peak boundaries"
            );
            config.colors.eventTriggers = Config.Bind(
                "Colors", "eventTriggers", colorEventTriggers,
                "The color to use for event triggers"
            );
            config.colors.windMillWings = Config.Bind(
                "Colors", "windMillWings", colorWindMillWings,
                "The color to use for windmill wings"
            );
            config.colors.timeAttack = Config.Bind(
                "Colors", "timeAttack", colorTimeAttack,
                "The color to use for time attack zones"
            );
            config.colors.windSectors = Config.Bind(
                "Colors", "windSectors", colorWindSectors,
                "The color to use for wind sectors"
            );
            config.colors.boxColliders = Config.Bind(
                "Colors", "boxColliders", colorBoxColliders,
                "The color to use for box colliders"
            );
            config.colors.capsuleColliders = Config.Bind(
                "Colors", "capsuleColliders", colorCapsuleColliders,
                "The color to use for capsule colliders"
            );
            config.colors.meshColliders = Config.Bind(
                "Colors", "meshColliders", colorMeshColliders,
                "The color to use for mesh colliders"
            );
            config.colors.sphereColliders = Config.Bind(
                "Colors", "sphereColliders", colorSphereColliders,
                "The color to use for sphere colliders"
            );
            config.colors.startRange = Config.Bind(
                "Colors", "startRange", colorStartRange,
                "The color to use for the start range"
            );
            config.colors.stamperRange = Config.Bind(
                "Colors", "stamperRange", colorStamperRange,
                "The color to use for the stamper range"
            );
            config.colors.summitRange = Config.Bind(
                "Colors", "summitRange", colorSummitRange,
                "The color to use for the summit range"
            );
            config.colors.summitLevel = Config.Bind(
                "Colors", "summitLevel", colorSummitLevel,
                "The color to use for the summit level"
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

        /**
         * <summary>
         * Executes when the plugin is destroyed.
         * </summary>
         */
        public void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /**
         * <summary>
         * Executes to render the GUI.
         * </summary>
         */
        public void OnGUI() {
            CommonGUI();
        }

        /**
         * <summary>
         * Executes on each frame.
         * </summary>
         */
        public void Update() {
            CommonUpdate();
        }

        /**
         * <summary>
         * Executes when a scene is loaded.
         * </summary>
         * <param name="scene">The scene which loaded</param>
         * <param name="mode">The mode the scene was loaded with</param>
         */
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            CommonSceneLoad();
        }

        /**
         * <summary>
         * Executes when a scene is unloaded.
         * </summary>
         * <param name="scene">The scene which unloaded</param>
         */
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

            // Custom colors
            MelonPreferences_Category colors = MelonPreferences.CreateCategory("MeshViewer_Colors");
            color.SetFilePath(filePath);

            config.colors.peakBoundaries = colors.CreateEntry<string>("peakBoundaries", colorPeakBoundaries);
            config.colors.eventTriggers = colors.CreateEntry<string>("eventTriggers", colorEventTriggers);
            config.colors.windMillWings = colors.CreateEntry<string>("windMillWings", colorWindMillWings);
            config.colors.timeAttack = colors.CreateEntry<string>("timeAttack", colorTimeAttack);
            config.colors.windSectors = colors.CreateEntry<string>("windSectors", colorWindSectors);
            config.colors.boxColliders = colors.CreateEntry<string>("boxColliders", colorBoxColliders);
            config.colors.capsuleColliders = colors.CreateEntry<string>("capsuleColliders", colorCapsuleColliders);
            config.colors.meshColliders = colors.CreateEntry<string>("meshColliders", colorMeshColliders);
            config.colors.sphereColliders = colors.CreateEntry<string>("sphereColliders", colorSphereColliders);
            config.colors.startRange = colors.CreateEntry<string>("startRange", colorStartRange);
            config.colors.stamperRange = colors.CreateEntry<string>("stamperRange", colorStamperRange);
            config.colors.summitRange = colors.CreateEntry<string>("summitRange", colorSummitRange);
            config.colors.summitLevel = colors.CreateEntry<string>("summitLevel", colorSummitLevel);

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

        /**
         * <summary>
         * Executes to render the GUI.
         * </summary>
         */
        public override void OnGUI() {
            CommonGUI();
        }

        /**
         * <summary>
         * Executes on each frame.
         * </summary>
         */
        public override void OnUpdate() {
            CommonUpdate();
        }

        /**
         * <summary>
         * Executes when a scene is loaded.
         * </summary>
         * <param name="buildIndex">The build index of the scene</param>
         * <param name="sceneName">The name of the scene</param>
         */
        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            CommonSceneLoad();
        }

        /**
         * <summary>
         * Executes when a scene is unloaded.
         * </summary>
         * <param name="buildIndex">The build index of the scene</param>
         * <param name="sceneName">The name of the scene</param>
         */
        public override void OnSceneWasUnloaded(int buildIndex, string sceneName) {
            CommonSceneUnload();
        }

#endif

        // Default colors
        private string colorPeakBoundaries   = RGBToString(224, 108, 117);
        private string colorEventTriggers    = RGBToString(229, 192, 123);
        private string colorWindMillWings    = RGBToString(152, 195, 121);
        private string colorTimeAttack       = RGBToString(86, 182, 194);
        private string colorWindSectors      = RGBToString(97, 175, 239);
        private string colorBoxColliders     = RGBToString(198, 120, 221);
        private string colorCapsuleColliders = RGBToString(193, 152, 193);
        private string colorMeshColliders    = RGBToString(122, 122, 122);
        private string colorSphereColliders  = RGBToString(204, 204, 204);
        private string colorStartRange       = RGBToString(128, 181, 189);
        private string colorStamperRange     = RGBToString(77, 94, 109);
        private string colorSummitRange      = RGBToString(200, 200, 200);
        private string colorSummitLevel      = RGBToString(103, 115, 126);

        // Default keybind for toggling the UI
        private KeyCode defaultToggleKeybind = KeyCode.Home;

        private Config.Cfg config = new Config.Cfg();
        private Cache cache;
        private UI ui;

        /**
         * <summary>
         * Converts RGB values into a color string.
         * </summary>
         * <param name="r">The red component</param>
         * <param name="g">The green component</param>
         * <param name="b">The blue component</param>
         * <return>The color string</return>
         */
        private static string RGBToString(int r, int g, int b) {
            return MeshViewer.Config.Colors.ColorToString(new Color(
                (float) r/255f, (float) g/255f, (float) b/255f)
            );
        }

        /**
         * <summary>
         * Common code to execute when the plugin is initialized.
         * </summary>
         */
        private void CommonAwake() {
            cache = new Cache(config);
            ui = new UI(cache);
        }

        /**
         * <summary>
         * Common code to execute to render the GUI.
         * </summary>
         */
        private void CommonGUI() {
            ui.Render();
        }

        /**
         * <summary>
         * Common code to execute on each frame.
         * </summary>
         */
        private void CommonUpdate() {
            ui.Update();
        }

        /**
         * <summary>
         * Common code to execute when a scene is loaded.
         * </summary>
         */
        public void CommonSceneLoad() {
            cache.CacheObjects();
            cache.Update();
        }

        /**
         * <summary>
         * Common code to execute when a scene is unloaded.
         * </summary>
         */
        public void CommonSceneUnload() {
            cache.Clear();
        }
    }
}
