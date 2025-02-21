using System;

using UnityEngine;

namespace MeshViewer {
    public class UI {
        private bool showUI;
        private Cache cache;

        private Config.Cfg config {
            get => cache.config;
        }

        public UI(Cache cache) {
            this.cache = cache;
            showUI = config.showUIByDefault.Value;
        }

        public void Update() {
            if (Input.GetKeyDown(config.toggleKeybind)) {
                showUI = !showUI;
            }
        }

        public void Render() {
            if (showUI == false) {
                return;
            }

            GUILayout.BeginArea(new Rect(10, 10, 150, 580), GUI.skin.box);

            Config.Render render = config.render;

            if (GUILayout.Button("Save") == true) {
                cache.Update();
            }

            GUILayout.Label("== Color ==");
            GUILayout.Label($"Red: {render.color.red.Value}");
            render.color.red.Value = (int) GUILayout.HorizontalSlider(
                render.color.red.Value, 0f, 255f
            );

            GUILayout.Label($"Green: {render.color.green.Value}");
            render.color.green.Value = (int) GUILayout.HorizontalSlider(
                render.color.green.Value, 0f, 255f
            );

            GUILayout.Label($"Blue: {render.color.blue.Value}");
            render.color.blue.Value = (int) GUILayout.HorizontalSlider(
                render.color.blue.Value, 0f, 255f
            );

            GUILayout.Label("== Render ==");
            render.peakBoundaries.Value = GUILayout.Toggle(
                render.peakBoundaries.Value, "Peak Boundaries"
            );
            render.eventTriggers.Value = GUILayout.Toggle(
                render.eventTriggers.Value, "Event Triggers"
            );
            render.windMillWings.Value = GUILayout.Toggle(
                render.windMillWings.Value, "Windmill Wings"
            );
            render.timeAttack.Value = GUILayout.Toggle(
                render.timeAttack.Value, "Time Attack"
            );
            render.windSectors.Value = GUILayout.Toggle(
                render.windSectors.Value, "Wind Sectors (ST)"
            );

            GUILayout.Label("== Colliders ==");
            Config.Colliders colliders = config.render.colliders;

            colliders.boxColliders.Value = GUILayout.Toggle(
                colliders.boxColliders.Value, "Box Colliders"
            );
            colliders.capsuleColliders.Value = GUILayout.Toggle(
                colliders.capsuleColliders.Value, "Capsule Colliders"
            );
            colliders.meshColliders.Value = GUILayout.Toggle(
                colliders.meshColliders.Value, "Mesh Colliders"
            );
            colliders.sphereColliders.Value = GUILayout.Toggle(
                colliders.sphereColliders.Value, "Sphere Colliders"
            );

            GUILayout.Label("== Summit Stuff ==");
            Config.SummitStuff summitStuff = config.render.summitStuff;

            GUILayout.Label($"Opacity ({summitStuff.opacity.Value})");
            summitStuff.opacity.Value = (float) Math.Round(GUILayout.HorizontalSlider(
                summitStuff.opacity.Value, 0f, 1f), 2
            );

            summitStuff.startRange.Value = GUILayout.Toggle(
                summitStuff.startRange.Value, "Start Range"
            );
            summitStuff.stamperRange.Value = GUILayout.Toggle(
                summitStuff.stamperRange.Value, "Stamper Range"
            );
            summitStuff.summitRange.Value = GUILayout.Toggle(
                summitStuff.summitRange.Value, "Summit Range"
            );
            summitStuff.summitLevel.Value = GUILayout.Toggle(
                summitStuff.summitLevel.Value, "Summit Level"
            );

            GUILayout.EndArea();
        }
    }
}
