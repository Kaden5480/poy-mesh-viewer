using UnityEngine;

namespace MeshViewer {
    public class RenderData {
        public GameObject parent { get; }
        public RenderType renderType { get; }

        private bool visible = false;
        private GameObject obj = null;
        private Renderer renderer = null;
        private Material normalMaterial = null;
        private Material renderMaterial = null;

        /**
         * <summary>
         * Constructs an instance of RenderData.
         *
         * The `parent` is used for determining the type of this object in more detail
         * than the type of collider which is on it. Such as whether it is on the
         * PeakBoundary, WindMillWings, or EventTrigger layer, what components it has, and so on.
         *
         * `obj` is the object which contains the renderer.
         * </summary>
         * <param name="parent">The parent of obj</param>
         * <param name="obj">The object which contains the renderer</param>
         * <param name="renderType">What kind of object this is</param>
         * <param name="material">The material to apply to the renderer when this object is shown</param>
         * <param name="visible">Whether this object is visible by default</param>
         */
        public RenderData(GameObject parent, GameObject obj, RenderType renderType, Material material, bool visible = false) {
            this.parent = parent;
            this.obj = obj;
            this.renderType = renderType;
            this.visible = visible;
            renderMaterial = material;

            renderer = obj.GetComponent<Renderer>();

            // If this is a visible object, store its normal material
            if (visible == true) {
                normalMaterial = renderer.material;
            }
            else {
                renderer.material = renderMaterial;
            }
        }

        /**
         * <summary>
         * Show this object with the provided color string.
         * </summary>
         * <param name="colorString">The color string to apply to this object's renderer</param>
         */
        public void Show(string colorString) {
            Color color = Config.Colors.StringToColor(colorString);

            // If this is a visible object, just swap the material
            if (visible == true) {
                renderer.material = renderMaterial;
            }
            // Otherwise, display the custom made renderer
            else {
                obj.SetActive(true);
            }

            // Also update the color of the material
            renderer.material.color = color;
        }

        /**
         * <summary>
         * Hides this object.
         * </summary>
         */
        public void Hide() {
            // If this isn't a visible object, disable the custom
            // made renderer
            if (visible == false) {
                obj.SetActive(false);
                return;
            }

            // Just to be sure, check the renderer exists
            if (renderer == null) {
                return;
            }

            // Restore the default material for the visible object
            renderer.material = normalMaterial;
        }
    }
}
