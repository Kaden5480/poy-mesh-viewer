using UnityEngine;

namespace MeshViewer {
    public class RenderData {
        public GameObject parent { get; }
        public RenderType renderType { get; }

        private GameObject obj = null;
        private Renderer renderer = null;
        private Material normalMaterial = null;
        private Material renderMaterial = null;

        public RenderData(GameObject parent, GameObject obj, RenderType renderType, Material material) {
            this.parent = parent;
            this.obj = obj;
            this.renderType = renderType;
            renderMaterial = material;

            renderer = obj.GetComponent<Renderer>();

            if (renderType == RenderType.MeshColliderVisible) {
                normalMaterial = renderer.material;
            }
            else {
                renderer.material = renderMaterial;
            }
        }

        public void Show(Color color) {
            if (renderType == RenderType.MeshColliderVisible) {
                renderer.material = renderMaterial;
            }
            else {
                obj.SetActive(true);
            }

            renderer.material.color = color;
        }

        public void Hide() {
            if (renderType != RenderType.MeshColliderVisible) {
                obj.SetActive(false);
                return;
            }

            if (renderer == null) {
                return;
            }

            renderer.material = normalMaterial;
        }
    }
}
