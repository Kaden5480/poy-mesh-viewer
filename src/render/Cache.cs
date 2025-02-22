using System.Collections.Generic;


#if BEPINEX
using BepInEx.Configuration;

#elif MELONLOADER
using MelonLoader;

#endif

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
         * Adds a GameObject which has a visible renderer to the cache.
         * </summary>
         * <param name="obj">The object to add</param>
         * <param name="renderType">The type of collider</param>
         * <return>True if the object is visible and was cached, false otherwise</return>
         */
        private bool CacheVisibleCollider(GameObject obj, RenderType renderType) {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

            // Check if the mesh is already visible
            if (renderer != null && renderer.enabled == true) {
                cache.Add(new RenderData(obj, obj, renderType, MakeMaterial(), true));
                return true;
            }

            return false;
        }

        /**
         * <summary>
         * Adds a GameObject with a BoxCollider to the cache.
         * </summary>
         * <param name="obj">The object to add</param>
         * <param name="collider">The collider to cache</param>
         */
        private void CacheBoxCollider(GameObject obj, BoxCollider collider) {
            // Check if already visible
            if (CacheVisibleCollider(obj, RenderType.BoxCollider) == true) {
                return;
            }

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
            // Check if already visible
            if (CacheVisibleCollider(obj, RenderType.SphereCollider) == true) {
                return;
            }

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
            // Check if already visible
            if (CacheVisibleCollider(obj, RenderType.MeshCollider) == true) {
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

            cache.Add(new RenderData(obj, child, RenderType.MeshCollider, MakeMaterial()));
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
         * Updates the rendering of a specific object.
         * </summary>
         * <param name="data">The data for the object</param>
         * <param name="enabled">Whether rendering this object is enabled</param>
         * <param name="color">The color to render this object with</param>
         */
#if BEPINEX
        private void UpdateObject(
            RenderData data,
            ConfigEntry<bool> enabled,
            ConfigEntry<string> color
        ) {
#elif MELONLOADER
        private void UpdateObject(
            RenderData data,
            MelonPreferences_Entry<bool> enabled,
            MelonPreferences_Entry<string> color
        ) {
#endif
            if (enabled.Value == false) {
                data.Hide();
                return;
            }

            data.Show(color.Value);
        }

        /**
         * <summary>
         * Updates the rendering of cached objects.
         * </summary>
         */
        public void Update() {
            Config.Colors colors = config.colors;
            Config.Colliders colliders = config.render.colliders;
            Config.Render render = config.render;
            Config.SummitStuff summitStuff = config.render.summitStuff;

            // If rendering is disabled, just hide everything
            if (config.enabled.Value == false) {
                foreach (RenderData data in cache) {
                    data.Hide();
                }
                return;
            }

            // For each type of object, update whether it is displayed
            // and also the color which it will be displayed with

            foreach (RenderData data in cache) {
                // Render
                if (data.parent.layer == LayerMask.NameToLayer("PlayerPhysics")) {
                    UpdateObject(data, render.playerPhysics, colors.playerPhysics);
                }
                else if ("PlayerTrigger".Equals(data.parent.tag) == true) {
                    UpdateObject(data, render.playerTriggers, colors.playerTriggers);
                }
                else if (data.parent.layer == LayerMask.NameToLayer("PeakBoundary")) {
                    UpdateObject(data, render.peakBoundaries, colors.peakBoundaries);
                }
                else if (data.parent.layer == LayerMask.NameToLayer("EventTrigger")) {
                    TimeAttackZone timeAttack = data.parent.GetComponent<TimeAttackZone>();
                    PeakWindSolemnTempest peakWind = data.parent.GetComponent<PeakWindSolemnTempest>();

                    // If this isn't a time attack zone/peak wind, it's another kind
                    // of event trigger
                    if (timeAttack == null && peakWind == null) {
                        UpdateObject(data, render.eventTriggers, colors.eventTriggers);
                    }
                    else if (timeAttack != null) {
                        UpdateObject(data, render.timeAttack, colors.timeAttack);
                    }
                    else if (peakWind != null) {
                        UpdateObject(data, render.windSectors, colors.windSectors);
                    }
                }
                else if (data.parent.layer == LayerMask.NameToLayer("WindMillWings")) {
                    UpdateObject(data, render.windMillWings, colors.windMillWings);
                }
                // Colliders
                else if (data.renderType == RenderType.BoxCollider) {
                    UpdateObject(data, colliders.boxColliders, colors.boxColliders);
                }
                else if (data.renderType == RenderType.CapsuleCollider) {
                    UpdateObject(data, colliders.capsuleColliders, colors.capsuleColliders);
                }
                else if (data.renderType == RenderType.MeshCollider) {
                    UpdateObject(data, colliders.meshColliders, colors.meshColliders);
                }
                else if (data.renderType == RenderType.SphereCollider) {
                    UpdateObject(data, colliders.sphereColliders, colors.sphereColliders);
                }
                // Summit stuff
                else if (data.renderType == RenderType.SummitStuffStart) {
                    UpdateObject(data, summitStuff.startRange, colors.startRange);
                }
                else if (data.renderType == RenderType.SummitStuffStamper) {
                    UpdateObject(data, summitStuff.stamperRange, colors.stamperRange);
                }
                else if (data.renderType == RenderType.SummitStuffSummitLevel) {
                    UpdateObject(data, summitStuff.summitLevel, colors.summitLevel);
                }
                else if (data.renderType == RenderType.SummitStuffSummitRange) {
                    UpdateObject(data, summitStuff.summitRange, colors.summitRange);
                }
            }
        }
    }
}
