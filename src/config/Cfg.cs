using UnityEngine;

#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace MeshViewer.Config {
    public struct Cfg {
        public Render render;

        public KeyCode toggleKeybind {
            get => (KeyCode) System.Enum.Parse(typeof(KeyCode), _toggleKeybind.Value);
            set {
                _toggleKeybind.Value = value.ToString();
            }
        }

#if BEPINEX
        public ConfigEntry<string> _toggleKeybind;
        public ConfigEntry<bool> showUIByDefault;

#elif MELONLOADER
        public MelonPreferences_Entry<string> _toggleKeybind;
        public MelonPreferences_Entry<bool> showUIByDefault;

#endif
    }
}
