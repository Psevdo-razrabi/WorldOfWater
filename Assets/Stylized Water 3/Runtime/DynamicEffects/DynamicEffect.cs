// Stylized Water 3 by Staggart Creations (http://staggart.xyz)
// COPYRIGHT PROTECTED UNDER THE UNITY ASSET STORE EULA (https://unity.com/legal/as-terms)
//    • Copying or referencing source code for the production of new asset store, or public, content is strictly prohibited!
//    • Uploading this file to a public repository will subject it to an automated DMCA takedown request.

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace StylizedWater3
{
    [HelpURL("https://staggart.xyz/unity/stylized-water-3/sw3-dynamic-effects-docs/")]
    [AddComponentMenu("Stylized Water 3/Dynamic Water Effect")]
    [Icon("Assets/Stylized Water 3/Editor/Resources/DynamicEffects/dynamic-effect-icon-256px.png")]
    public class DynamicEffect : MonoBehaviour
    {
        #pragma warning disable 108,114 //New keyword
        public Renderer renderer; 
        #pragma warning restore 108,114

        [Tooltip("Higher layers are always drawn over lower layers. Use this to override other effects on a lower layer.\n\nThis is effectively the render queue")]
        public int sortingLayer = 0;

        /// <summary>
        /// Material used for this effect
        /// </summary>
        public Material templateMaterial;
        private Material material;

        [UnityEngine.Serialization.FormerlySerializedAs("displacementScale")]
        public float heightScale = 1f;
        public bool scaleHeightByTransform;
        [Min(0f)]
        public float foamAmount = 1f;
        [Min(0f)]
        public float normalStrength = 1f;
        public bool scaleNormalByHeight;

        [SerializeField]
        private ParticleSystemRenderer particleSystemRenderer;
        
        private void Reset()
        {
            renderer = GetComponent<Renderer>();

            particleSystemRenderer = renderer.GetComponent<ParticleSystemRenderer>();

            if (!renderer)
            {
                DestroyImmediate(this);
                throw new Exception("Component must only be added to a GameObject with a renderer (Mesh Renderer, Trail Renderer, Line Renderer or Particle System)");
            }

            TryGetTemplateMaterial();
        }

        private void TryGetTemplateMaterial()
        {
            if (particleSystemRenderer)
            {
                templateMaterial = particleSystemRenderer.sharedMaterial;
                return;
            }
            
            templateMaterial = renderer.sharedMaterial;
        }
        
        void Start()
        {
            UpdateMaterial();
        }
        
        private void OnValidate()
        {
            UpdateMaterial();
            
            //Upgrade to SW3
            if (!templateMaterial) TryGetTemplateMaterial();
        }

        private void OnDestroy()
        {
            CoreUtils.Destroy(material);
        }

        private static readonly int _HeightScale = Shader.PropertyToID("_HeightScale");
        private static readonly int _FoamStrength = Shader.PropertyToID("_FoamStrength");
        private static readonly int _NormalStrength = Shader.PropertyToID("_NormalStrength");

		[Obsolete("Use UpdateMaterial() instead")]
		public void UpdateProperties()
		{
			UpdateMaterial();
		}
		
        /// <summary>
        /// Copies the properties from the <see cref="templateMaterial">Template Material</see> but overwrites the Height/Foam/Normal values as configured on this component
        /// Creates a new material instance if necessary.
        /// </summary>
        public void UpdateMaterial()
        {
            if (!templateMaterial || !renderer) return;

            var parentChanged = false;
            
            #if UNITY_EDITOR
            parentChanged = material && templateMaterial.parent != material.parent;
            #endif
            
            if (!material || parentChanged)
            {
                material = new Material(templateMaterial);
                #if UNITY_EDITOR
                //This allows user edits on the template material to carry over to the instantiated material in the editor
                material.parent = templateMaterial;
                #endif
                material.name += " (Instance)";
                renderer.material = material;

                material.hideFlags = HideFlags.NotEditable;
                
                if (particleSystemRenderer) particleSystemRenderer.material = material;
            }
            
            material.CopyPropertiesFromMaterial(templateMaterial);

            var height = heightScale * (scaleHeightByTransform ? this.transform.lossyScale.y : 1f);
            material.SetFloat(_HeightScale, height);
            material.SetFloat(_FoamStrength, foamAmount);
            material.SetFloat(_NormalStrength, normalStrength * (scaleNormalByHeight ? height : 1f));
            
            renderer.sortingOrder = sortingLayer;
        }

        #if UNITY_EDITOR
        [ContextMenu("Open Rendering Debugger")]
        private void OpenDebugger()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem("Window/Analysis/Stylized Water 3/Render targets");
        }
        #endif
    }
}