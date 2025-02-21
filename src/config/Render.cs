#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace MeshViewer.Config {
    public struct Render {
        public Color color;
        public Colliders colliders;
        public SummitStuff summitStuff;

#if BEPINEX
        public ConfigEntry<bool> peakBoundaries;
        public ConfigEntry<bool> eventTriggers;
        public ConfigEntry<bool> windMillWings;
        public ConfigEntry<bool> timeAttack;
        public ConfigEntry<bool> windSectors;

#elif MELONLOADER
        public MelonPreferences_Entry<bool> peakBoundaries;
        public MelonPreferences_Entry<bool> eventTriggers;
        public MelonPreferences_Entry<bool> windMillWings;
        public MelonPreferences_Entry<bool> timeAttack;
        public MelonPreferences_Entry<bool> windSectors;

#endif
    }
}
