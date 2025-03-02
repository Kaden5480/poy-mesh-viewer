using System;
using System.Collections.Generic;

using UnityEngine;

#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

namespace MeshViewer {
    public class UI {
        private bool showUI;
        private bool allowingMovement = true;
        private Cache cache;

        // Size info for the GUI
        const int width = 300;
        const int height = 400;
        const int padding = 20;
        const int buttonWidth = 100;

        private Vector2 scrollPosition = Vector2.zero;

        // Store which colors are being picked
        private Dictionary<string, bool> colorPicker = new Dictionary<string, bool>();

        private Config.Cfg config {
            get => cache.config;
        }

        /**
         * <summary>
         * Toggles whether movement is allowed.
         * </summary>
         */
        private void AllowMovement(bool allow) {
            allowingMovement = allow;

            if (cache.playerManager != null) {
                cache.playerManager.AllowPlayerControl(allow);
            }

            if (cache.peakSummited != null) {
                cache.peakSummited.DisableEverythingButClimbing(!allow);
            }

        }

        /**
         * <summary>
         * Sets the cursor lock state.
         * </summary>
         */
        private void SetCursorLock() {
            if (InGameMenu.isLoading == true
                || EnterPeakScene.enteringPeakScene == true
                || EnterPeakScene.enteringAlpScene == true
                || EnterRoomSegmentScene.enteringScene == true
            ) {
                return;
            }

            if (showUI == true) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                InGameMenu.hasBeenInMenu = true;
                AllowMovement(false);
            }
            else if (cache.inGameMenu != null &&
                (cache.inGameMenu.isMainMenu == true || cache.inGameMenu.inMenu == true)
            ) {
                return;
            } else if (InGameMenu.isCurrentlyNavigationMenu == true) {
                return;
            } else if (allowingMovement == false && showUI == false) {
                AllowMovement(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        /**
         * <summary>
         * Constructs an instance of UI.
         * </summary>
         * <param name="cache">The cache of all objects in the scene</param>
         */
        public UI(Cache cache) {
            this.cache = cache;
            showUI = config.showUIByDefault.Value;
        }

        /**
         * <summary>
         * Executes each frame to check for a toggle input.
         * </summary>
         */
        public void Update() {
            if (Input.GetKeyDown(config.toggleKeybind)) {
                showUI = !showUI;
                SetCursorLock();
            }
        }

        /**
         * <summary>
         * Renders options for a specific toggle + custom color
         * </summary>
         * <param name="text">The name to display for this option</param>
         * <param name="enabled">Whether rendering of this option is enabled</param>
         * <param name="colorConfig">The custom color for this option</param>
         * <param name="canModifyAlpha">Whether the alpha value can be modified for this option</param>
         */
#if BEPINEX
        private void RenderOption(
            string text,
            ConfigEntry<bool> enabled,
            ConfigEntry<string> colorConfig,
            bool canModifyAlpha = false
        ) {
#elif MELONLOADER
        private void RenderOption(
            string text,
            MelonPreferences_Entry<bool> enabled,
            MelonPreferences_Entry<string> colorConfig,
            bool canModifyAlpha = false
        ) {
#endif
            if (colorPicker.ContainsKey(text) == false) {
                // Insert default value if it doesn't exist
                colorPicker[text] = false;
            }

            GUILayout.BeginHorizontal();

            // Show set color/done depending on whether the color picker
            // is in use for this option
            if (colorPicker[text] == false
                && GUILayout.Button("Set Color", GUILayout.Width(buttonWidth))
            ) {
                colorPicker[text] = true;
            }
            else if (colorPicker[text] == true
                && GUILayout.Button("Done", GUILayout.Width(buttonWidth))
            ) {
                colorPicker[text] = false;
            }

            enabled.Value = GUILayout.Toggle(
                enabled.Value, text
            );

            GUILayout.EndHorizontal();

            if (colorPicker[text] == false) {
                return;
            }

            // If the color picker is in use, render it
            Color color = Config.Colors.StringToColor(
                colorConfig.Value
            );

            int red = (int) (255f * color.r);
            int green = (int) (255f * color.g);
            int blue = (int) (255f * color.b);
            float alpha = color.a;

            GUILayout.Label($"Red: {red}");
            red = (int) GUILayout.HorizontalSlider(
                red, 0f, 255f
            );
            GUILayout.Label($"Green: {green}");
            green = (int) GUILayout.HorizontalSlider(
                green, 0f, 255f
            );
            GUILayout.Label($"Blue: {blue}");
            blue = (int) GUILayout.HorizontalSlider(
                blue, 0f, 255f
            );

            // If the alpha value of the color can be modified
            // also render it
            if (canModifyAlpha == true) {
                GUILayout.Label($"Alpha: {alpha}");
                alpha = (float) Math.Round(GUILayout.HorizontalSlider(
                    alpha, 0f, 1f
                ), 2);
            }

            // Check if restoring the default color was picked
            if (GUILayout.Button("Restore Default Color") == true) {
                colorConfig.Value = (string) colorConfig.DefaultValue;
            }
            else {
                color = new Color(
                    (float) red / 255f,
                    (float) green / 255f,
                    (float) blue / 255f,
                    alpha
                );
                colorConfig.Value = Config.Colors.ColorToString(color);
            }
        }

        /**
         * <summary>
         * Renders the GUI.
         * </summary>
         */
        public void Render() {
            if (showUI == false) {
                return;
            }

            SetCursorLock();

            // Display everything in a box with a scroll view
            GUILayout.BeginArea(new Rect(10, 10, width, height), GUI.skin.box);

            scrollPosition = GUILayout.BeginScrollView(
                scrollPosition,
                GUILayout.Width(width - padding), GUILayout.Height(height - padding - 37)
            );

            Config.Colors colors = config.colors;

            // Allow enabling/disabling all rendering
            GUILayout.Label("== Global ==");
            if (config.enabled.Value == false
                && GUILayout.Button(
                    "Enable", GUILayout.Width(buttonWidth)
                ) == true
            ) {
                config.enabled.Value = true;
                cache.Update();
            }
            else if (config.enabled.Value == true
                && GUILayout.Button(
                    "Disable", GUILayout.Width(buttonWidth)
                ) == true
            ) {
                config.enabled.Value = false;
                cache.Update();
            }

            GUILayout.Label("== Categorized ==");
            Config.Render render = config.render;
            RenderOption("Peak Boundaries", render.peakBoundaries, colors.peakBoundaries);
            RenderOption("Event Triggers", render.eventTriggers, colors.eventTriggers);
            RenderOption("Windmill Wings", render.windMillWings, colors.windMillWings);
            RenderOption("Time Attack", render.timeAttack, colors.timeAttack);
            RenderOption("Wind Sectors (ST)", render.windSectors, colors.windSectors);
            RenderOption("Player Physics", render.playerPhysics, colors.playerPhysics);
            RenderOption("Player Triggers", render.playerTriggers, colors.playerTriggers);

            GUILayout.Label("== Uncategorized ==");
            Config.Colliders colliders = config.render.colliders;
            RenderOption("Box Colliders", colliders.boxColliders, colors.boxColliders);
            RenderOption("Capsule Colliders", colliders.capsuleColliders, colors.capsuleColliders);
            RenderOption("Mesh Colliders", colliders.meshColliders, colors.meshColliders);
            RenderOption("Sphere Colliders", colliders.sphereColliders, colors.sphereColliders);

            GUILayout.Label("== Summit Stuff ==");
            Config.SummitStuff summitStuff = config.render.summitStuff;
            // Summit stuff can have custom alpha values (at least for now)
            RenderOption("Start Range", summitStuff.startRange, colors.startRange, true);
            RenderOption("Stamper Range", summitStuff.stamperRange, colors.stamperRange, true);
            RenderOption("Summit Range", summitStuff.summitRange, colors.summitRange, true);
            RenderOption("Summit Level", summitStuff.summitLevel, colors.summitLevel, true);

            GUILayout.EndScrollView();


            // Update displayed objects if clicked
            if (GUILayout.Button("Update") == true) {
                cache.Update();
            }

            if (GUILayout.Button("Close") == true) {
                showUI = false;
                SetCursorLock();
            }

            GUILayout.EndArea();
        }
    }
}
