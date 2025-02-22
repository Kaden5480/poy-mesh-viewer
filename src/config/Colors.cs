using UnityEngine;

#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace MeshViewer.Config {
    public struct Colors {
        /**
         * <summary>
         * Converts a string to a Color.
         * </summary>
         * <param name="colorString">The color string to convert</param>
         * <return>The color</return>
         */
        public static Color StringToColor(string colorString) {
            ColorUtility.TryParseHtmlString($"#{colorString}", out Color color);
            return color;
        }

        /**
         * <summary>
         * Converts a Color to a string.
         * </summary>
         * <param name="color">The color to convert</param>
         * <return>The string</return>
         */
        public static string ColorToString(Color color) {
            return ColorUtility.ToHtmlStringRGBA(color);
        }

#if BEPINEX
        // Render
        public ConfigEntry<string> peakBoundaries;
        public ConfigEntry<string> eventTriggers;
        public ConfigEntry<string> windMillWings;
        public ConfigEntry<string> timeAttack;
        public ConfigEntry<string> windSectors;

        // Colliders
        public ConfigEntry<string> boxColliders;
        public ConfigEntry<string> capsuleColliders;
        public ConfigEntry<string> meshColliders;
        public ConfigEntry<string> sphereColliders;

        // Summit stuff
        public ConfigEntry<string> startRange;
        public ConfigEntry<string> stamperRange;
        public ConfigEntry<string> summitRange;
        public ConfigEntry<string> summitLevel;

#elif MELONLOADER
        // Render
        public MelonPreferences_Entry<string> peakBoundaries;
        public MelonPreferences_Entry<string> eventTriggers;
        public MelonPreferences_Entry<string> windMillWings;
        public MelonPreferences_Entry<string> timeAttack;
        public MelonPreferences_Entry<string> windSectors;

        // Colliders
        public MelonPreferences_Entry<string> boxColliders;
        public MelonPreferences_Entry<string> capsuleColliders;
        public MelonPreferences_Entry<string> meshColliders;
        public MelonPreferences_Entry<string> sphereColliders;

        // Summit stuff
        public MelonPreferences_Entry<string> startRange;
        public MelonPreferences_Entry<string> stamperRange;
        public MelonPreferences_Entry<string> summitRange;
        public MelonPreferences_Entry<string> summitLevel;

#endif
    }
}
