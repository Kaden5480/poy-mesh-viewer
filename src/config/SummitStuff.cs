#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace MeshViewer.Config {
    public struct SummitStuff {

#if BEPINEX
        public ConfigEntry<float> opacity;
        public ConfigEntry<bool> startRange;
        public ConfigEntry<bool> stamperRange;
        public ConfigEntry<bool> summitRange;
        public ConfigEntry<bool> summitLevel;

#elif MELONLOADER
        public MelonPreferences_Entry<float> opacity;
        public MelonPreferences_Entry<bool> startRange;
        public MelonPreferences_Entry<bool> stamperRange;
        public MelonPreferences_Entry<bool> summitRange;
        public MelonPreferences_Entry<bool> summitLevel;

#endif
    }
}
