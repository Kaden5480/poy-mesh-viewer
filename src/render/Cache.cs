using System.Collections.Generic;

using UnityEngine;

using Colors = MeshViewer.Config.Colors;

namespace MeshViewer {
    public class Cache {
        public Config.Cfg config { get; }
        private List<RenderData> cache;

        /**
         * <summary>
         * Constructs an instance of Cache.
         * </summary>
         * <parma name="config">The user config</param>
         */
        public Cache(Config.Cfg config) {
            this.config = config;

            // Initialize the cache
            cache = new List<RenderData>();
        }

        /**
         * <summary>
         * Makes a material.
         * </summary>
         * <return>The material</return>
         */
        private Material MakeMaterial() {
            return new Material(Shader.Find("Standard"));
        }

        /**
         * <summary>
         * Makes a transparent material.
         * This is only used for the "Summit Stuff" category.
         * </summary>
         * <return>The material</return>
         */
        private Material MakeTransparent() {
            Material material = MakeMaterial();

            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_CullMode", (int)UnityEngine.Rendering.CullMode.Off);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            return material;
        }

        /**
         * <summary>
         * Adds a GameObject with a BoxCollider to the cache.
         * </summary>
         * <param name="obj">The object to add</param>
         * <param name="collider">The collider to cache</param>
         */
        private void CacheBoxCollider(GameObject obj, BoxCollider collider) {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.DestroyImmediate(child.GetComponent<BoxCollider>());

            child.name = "Box Collider Viewer";

            child.transform.SetParent(obj.transform);
            child.transform.localRotation = Quaternion.identity;
            child.transform.localPosition = collider.center;
            child.transform.localScale = collider.size;

            cache.Add(new RenderData(obj, child, RenderType.BoxCollider, MakeMaterial()));
        }

        /**
         * <summary>
         * Adds a GameObject with a CapsuleCollider to the cache.
         * </summary>
         * <param name="obj">The object to add</param>
         * <param name="collider">The collider to cache</param>
         */
        private void CacheCapsuleCollider(GameObject obj, CapsuleCollider collider) {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            GameObject.DestroyImmediate(child.GetComponent<CapsuleCollider>());

            child.name = "Capsule Collider Viewer";

            child.transform.SetParent(obj.transform);
            child.transform.localPosition = collider.center;
            child.transform.localScale = new Vector3(
                collider.radius * 2,
                collider.height / 2,
                collider.radius * 2
            );

            switch (collider.direction) {
                case 0:
                    child.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 1:
                    child.transform.localRotation = Quaternion.identity;
                    break;
                case 2:
                    child.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    break;
            }

            cache.Add(new RenderData(obj, child, RenderType.CapsuleCollider, MakeMaterial()));
        }

        /**
         * <summary>
         * Adds a GameObject with a SphereCollider to the cache.
         * </summary>
         * <param name="obj">The object to add</param>
         * <param name="collider">The collider to cache</param>
         */
        private void CacheSphereCollider(GameObject obj, SphereCollider collider) {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject.DestroyImmediate(child.GetComponent<SphereCollider>());

            child.name = "Sphere Collider Viewer";

            child.transform.SetParent(obj.transform);
            child.transform.localPosition = collider.center;
            child.transform.localScale = 2 * Vector3.one * collider.radius;

            cache.Add(new RenderData(obj, child, RenderType.SphereCollider, MakeMaterial()));
        }

        /**
         * <summary>
         * Adds a GameObject with a MeshCollider to the cache.
         * </summary>
         * <param name="obj">The object to add</param>
         * <param name="collider">The collider to cache</param>
         */
        private void CacheMeshCollider(GameObject obj, MeshCollider collider) {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

            // Check if the mesh is already visible
            if (renderer != null && renderer.enabled == true) {
                cache.Add(new RenderData(obj, obj, RenderType.MeshColliderVisible, MakeMaterial()));
                return;
            }

            GameObject child = new GameObject("Mesh Collider Viewer");

            child.transform.SetParent(obj.transform);
            child.transform.position = obj.transform.position;
            child.transform.rotation = obj.transform.rotation;
            child.transform.localScale = Vector3.one;

            MeshFilter filter = child.AddComponent<MeshFilter>();
            MeshRenderer childRenderer = child.AddComponent<MeshRenderer>();

            filter.mesh = collider.sharedMesh;

            cache.Add(new RenderData(obj, child, RenderType.MeshColliderInvisible, MakeMaterial()));
        }

        /**
         * <summary>
         * Caches a plane.
         * </summary>
         * <param name="target">Where to render the plane</param>
         * <param name="size">The size of the plane</param>
         */
        private void CachePlane(Transform target, float size, RenderType renderType) {
            if (target == null) {
                return;
            }

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.DestroyImmediate(obj.GetComponent<BoxCollider>());

            Vector3 transform = target.position;
            transform.y -= 1.4f;

            obj.transform.position = transform;
            obj.transform.localScale = new Vector3(size, 0.00001f, size);

            cache.Add(new RenderData(obj, obj, renderType, MakeTransparent()));
        }

        /**
         * <summary>
         * Caches a sphere.
         * </summary>
         * <param name="target">Where to render the sphere</param>
         * <param name="radius">The radius of the sphere</param>
         */
        private void CacheSphere(Transform target, float radius, RenderType renderType) {
            if (target == null) {
                return;
            }

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject.DestroyImmediate(obj.GetComponent<SphereCollider>());

            obj.transform.position = target.position;
            obj.transform.localScale = Vector3.one * radius * 2;

            cache.Add(new RenderData(obj, obj, renderType, MakeTransparent()));
        }

        /**
         * <summary>
         * Caches the specified object.
         * </summary>
         * <param name="obj">The object to cache</param>
         */
        private void CacheObject(GameObject obj) {
            foreach (BoxCollider collider in obj.GetComponents<BoxCollider>()) {
                CacheBoxCollider(obj, collider);
            }

            foreach (CapsuleCollider collider in obj.GetComponents<CapsuleCollider>()) {
                CacheCapsuleCollider(obj, collider);
            }

            foreach (SphereCollider collider in obj.GetComponents<SphereCollider>()) {
                CacheSphereCollider(obj, collider);
            }

            foreach (MeshCollider collider in obj.GetComponents<MeshCollider>()) {
                CacheMeshCollider(obj, collider);
            }
        }

        /**
         * <summary>
         * Caches summit stuff.
         * </summary>
         */
        private void CacheSummitStuff() {
            GameObject summitObj = GameObject.Find("SUMMIT");
            GameObject camYObj = GameObject.FindGameObjectWithTag("MainCamera");
            GameObject summitBoxObj = GameObject.FindGameObjectWithTag("SummitBox");

            if (summitObj == null || camYObj == null || summitBoxObj == null) {
                return;
            }

            PeakSummited summit = summitObj.GetComponent<PeakSummited>();
            Transform camY = camYObj.GetComponent<Transform>();
            Transform summitBox = summitBoxObj.GetComponent<Transform>();

            CacheSphere(camY, 5.0f, RenderType.SummitStuffStart);
            CacheSphere(summitBox, 4.2f, RenderType.SummitStuffStamper);
            CacheSphere(summit.transform, 4.2f, RenderType.SummitStuffSummitRange);
            CachePlane(summit.transform, 20f, RenderType.SummitStuffSummitLevel);
        }

        /**
         * <summary>
         * Caches any objects of interest in the current scene.
         * </summary>
         */
        public void CacheObjects() {
            GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in objs) {
                if ("Player".Equals(obj.name) == true
                    || obj.layer == LayerMask.NameToLayer("PlayerPhysics")
                    || "PlayerTrigger".Equals(obj.tag) == true
                ) {
                    continue;
                }

                CacheObject(obj);
            }

            CacheSummitStuff();
        }

        /**
         * <summary>
         * Clears the cache.
         * </summary>
         */
        public void Clear() {
            cache.Clear();
        }

        /**
         * <summary>
         * Updates the rendering of cached objects.
         * </summary>
         */
        public void Update() {
            Config.Colors colors = config.colors;

            // For each type of object, update whether it is displayed
            // and also the color which it will be displayed with

            foreach (RenderData data in cache) {
                // Colliders
                Config.Colliders colliders = config.render.colliders;
                if (data.renderType == RenderType.BoxCollider
                    && colliders.boxColliders.Value == true
                ) {
                    data.Show(colors.boxColliders.Value);
                    continue;
                }

                if (data.renderType == RenderType.CapsuleCollider
                    && colliders.capsuleColliders.Value == true
                ) {
                    data.Show(colors.capsuleColliders.Value);
                    continue;
                }

                if (data.renderType == RenderType.MeshColliderInvisible
                    && colliders.meshColliders.Value == true
                ) {
                    data.Show(colors.meshColliders.Value);
                    continue;
                }

                if (data.renderType == RenderType.SphereCollider
                    && colliders.sphereColliders.Value == true
                ) {
                    data.Show(colors.sphereColliders.Value);
                    continue;
                }

                // Summit stuff
                Config.SummitStuff summitStuff = config.render.summitStuff;
                if (data.renderType == RenderType.SummitStuffStart
                    && summitStuff.startRange.Value == true
                ) {
                    data.Show(colors.startRange.Value);
                    continue;
                }

                if (data.renderType == RenderType.SummitStuffStamper
                    && summitStuff.stamperRange.Value == true
                ) {
                    data.Show(colors.stamperRange.Value);
                    continue;
                }

                if (data.renderType == RenderType.SummitStuffSummitLevel
                    && summitStuff.summitLevel.Value == true
                ) {
                    data.Show(colors.summitLevel.Value);
                    continue;
                }

                if (data.renderType == RenderType.SummitStuffSummitRange
                    && summitStuff.summitRange.Value == true
                ) {
                    data.Show(colors.summitRange.Value);
                    continue;
                }

                // Other rendering
                Config.Render render = config.render;
                if (data.parent.layer == LayerMask.NameToLayer("EventTrigger")) {
                    TimeAttackZone timeAttack = data.parent.GetComponent<TimeAttackZone>();
                    PeakWindSolemnTempest peakWind = data.parent.GetComponent<PeakWindSolemnTempest>();

                    // If this isn't a time attack zone/peak wind, it's another kind
                    // of event trigger
                    if (render.eventTriggers.Value == true
                        && timeAttack == null && peakWind == null
                    ) {
                        data.Show(colors.eventTriggers.Value);
                        continue;
                    }
                    else if (timeAttack != null
                        && render.timeAttack.Value == true
                    ) {
                        data.Show(colors.timeAttack.Value);
                        continue;
                    }
                    else if (peakWind != null
                        && render.windSectors.Value == true
                    ) {
                        data.Show(colors.windSectors.Value);
                        continue;
                    }
                }

                if (data.parent.layer == LayerMask.NameToLayer("PeakBoundary")
                    && render.peakBoundaries.Value == true
                ) {
                    data.Show(colors.peakBoundaries.Value);
                    continue;
                }

                if (data.parent.layer == LayerMask.NameToLayer("WindMillWings")
                    && render.windMillWings.Value == true
                ) {
                    data.Show(colors.windMillWings.Value);
                    continue;
                }

                // If this object wasn't selected to be rendered, just hide it
                data.Hide();
            }
        }
    }
}
