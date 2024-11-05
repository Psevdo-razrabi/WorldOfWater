// Stylized Water 3 by Staggart Creations (http://staggart.xyz)
// COPYRIGHT PROTECTED UNDER THE UNITY ASSET STORE EULA (https://unity.com/legal/as-terms)
//    • Copying or referencing source code for the production of new asset store, or public, content is strictly prohibited!
//    • Uploading this file to a public repository will subject it to an automated DMCA takedown request.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StylizedWater3
{
    public class DebugInspector
    {
        public class RenderTarget
        {
            public string name;
            public string textureName;
            public string description = string.Empty;
            public int order = 1000;

            protected int propertyID;
            public RenderTexture rt;

            public void Update()
            {
                //BROKEN SINCE 6000.0.16f1!
                //Changelog: "Universal RP: Disabled implicit use of all globals in URP passes"
                rt = Shader.GetGlobalTexture(propertyID) as RenderTexture;
            }
        }
        
        public List<RenderTarget> renderTargets = new List<RenderTarget>();
        //For dropdown menus
        public string[] renderTargetNames;

        private void Refresh()
        {
            renderTargets.Clear();
        
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsAbstract || type.IsInterface) continue;

                    if (type.IsSubclassOf(typeof(RenderTarget)))
                    {
                        //Debug.Log($"Found {type}");

                        RenderTarget rt = Activator.CreateInstance(type) as RenderTarget;
                    
                        //rt.Update();
                        renderTargets.Add(rt);
                    }
                }
            }

            renderTargets = renderTargets.OrderBy(o => o.order).ToList();

            renderTargetNames = new string[renderTargets.Count];
            for (int i = 0; i < renderTargetNames.Length; i++)
            {
                renderTargetNames[i] = renderTargets[i].name;
            }
            
        }

        public DebugInspector()
        {
            Refresh();
        }
    }
}