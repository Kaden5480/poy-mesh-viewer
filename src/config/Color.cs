#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace MeshViewer.Config {
    public struct Color {

#if BEPINEX
        public ConfigEntry<int> red;
        public ConfigEntry<int> green;
        public ConfigEntry<int> blue;

#elif MELONLOADER
        public MelonPreferences_Entry<int> red;
        public MelonPreferences_Entry<int> green;
        public MelonPreferences_Entry<int> blue;

#endif
    }
}
