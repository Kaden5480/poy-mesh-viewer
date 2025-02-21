#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace MeshViewer.Config {
    public struct Colliders {

#if BEPINEX
        public ConfigEntry<bool> boxColliders;
        public ConfigEntry<bool> capsuleColliders;
        public ConfigEntry<bool> meshColliders;
        public ConfigEntry<bool> sphereColliders;

#elif MELONLOADER
        public MelonPreferences_Entry<bool> boxColliders;
        public MelonPreferences_Entry<bool> capsuleColliders;
        public MelonPreferences_Entry<bool> meshColliders;
        public MelonPreferences_Entry<bool> sphereColliders;

#endif
    }
}
