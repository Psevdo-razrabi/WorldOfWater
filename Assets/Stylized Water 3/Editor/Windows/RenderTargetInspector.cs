// Stylized Water 3 by Staggart Creations (http://staggart.xyz)
// COPYRIGHT PROTECTED UNDER THE UNITY ASSET STORE EULA (https://unity.com/legal/as-terms)
//    • Copying or referencing source code for the production of new asset store, or public, content is strictly prohibited!
//    • Uploading this file to a public repository will subject it to an automated DMCA takedown request.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace StylizedWater3
{
    public class RenderTargetInspector : EditorWindow
    {
        private const int m_width = 550;
        private const int m_height = 300;
        
        [MenuItem("Window/Analysis/Stylized Water 3/Render targets", false, 0)]
        private static void OpenDebugger()
        {
            RenderTargetInspector.Open();
        }
        
        #if SWS_DEV
        [MenuItem("SWS/Debug/Render Targets")]
        #endif
        public static void Open()
        {
            RenderTargetInspector window = GetWindow<RenderTargetInspector>(false);
            window.titleContent = new GUIContent("Water Render Buffer Inspector");

            window.autoRepaintOnSceneChange = true;
            window.minSize = new Vector2(m_width, m_height);
            //window.maxSize = new Vector2(m_width, m_height);
            window.Show();
        }

        private float width = 300f;
        private Vector2 scrollPos;

        private ColorWriteMask colorMask = ColorWriteMask.All;
        private int colorChannel = 1;
        private int renderTargetIndex;
        private float exposure = 1f;

        private DebugInspector debugInspector;
        
        private void OnEnable()
        {
            debugInspector = new DebugInspector();
        }

        private void GetRenderTargets()
        {
            foreach (var renderTarget in debugInspector.renderTargets)
            {
                renderTarget.Update();
            }
        }
        
        private void OnGUI()
        {
            UI.DrawNotification("Sorry, this functionality was broken towards the end of the Unity 6 beta. None of the render targets can be fetched to view here anymore." +
                                "\n\nWill need to figure out how to get it working again later", MessageType.Error);
            Repaint();
            
            List<RenderGraph> rgs = RenderGraph.GetRegisteredRenderGraphs();

            

            if (debugInspector.renderTargetNames.Length < 5)
            {
                renderTargetIndex = GUILayout.Toolbar(renderTargetIndex, debugInspector.renderTargetNames);
            }
            else
            {
                renderTargetIndex = EditorGUILayout.Popup("Render target", renderTargetIndex, debugInspector.renderTargetNames);
            }
            
            width = (Mathf.Min(this.position.height, this.position.width) * 1f) - 15f;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            /*
            foreach (RenderGraph rg in rgs)
            {
                FieldInfo m_ResourcesField = typeof(RenderGraph).GetField("m_Resources", BindingFlags.NonPublic | BindingFlags.Instance);

                Assembly[] ass = AppDomain.CurrentDomain
                    .GetAssemblies();

                for (int i = 0; i < ass.Length; i++)
                {
                    //EditorGUILayout.TextField(ass[i].FullName);
                }

                Assembly assembly = Assembly.Load("Unity.RenderPipelines.Core.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    //EditorGUILayout.TextField(types[i].FullName);
                }

                Type RenderGraphResourceRegistryType = assembly.GetType("UnityEngine.Rendering.RenderGraphModule.RenderGraphResourceRegistry");
                EditorGUILayout.LabelField(RenderGraphResourceRegistryType.ToString());

                types = RenderGraphResourceRegistryType.GetNestedTypes(BindingFlags.NonPublic);
                
                Type RenderGraphResourcesDataType = RenderGraphResourceRegistryType.GetNestedType(
                    "UnityEngine.Rendering.RenderGraphModule.RenderGraphResourceRegistry+RenderGraphResourcesData", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i].Name.Contains("RenderGraphResourcesData"))
                    {
                        RenderGraphResourcesDataType = types[i];
                    }
                    EditorGUILayout.TextField(types[i].AssemblyQualifiedName);
                }
                EditorGUILayout.LabelField(RenderGraphResourcesDataType.ToString());
                object m_Resources = m_ResourcesField.GetValue(rg);

                Type IRenderGraphResourceType = assembly.GetType("UnityEngine.Rendering.RenderGraphModule.IRenderGraphResource");
                DynamicArray<RenderGraphResource> resourceArray;
                

                FieldInfo m_RenderGraphResourcesField = m_RenderGraphResourcesType.GetField("m_RenderGraphResources", BindingFlags.NonPublic | BindingFlags.Instance);

                object m_RenderGraphResources = m_RenderGraphResourcesField.GetValue(m_Resources);


            }
            */

            int currentTarget = 0;
            foreach (var renderTarget in debugInspector.renderTargets)
            {
                if (currentTarget == renderTargetIndex)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope())
                        {
                            renderTarget.Update();
                            DrawTexture(renderTarget);
                        }
                    }
                }

                currentTarget++;

            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawTexture(DebugInspector.RenderTarget renderTarget)
        {
            if (!renderTarget.rt)
            {
                EditorGUILayout.HelpBox($"Render target \"{renderTarget.textureName}\" couldn't be found, or it is not bound." +
                                        $"\n\nThe related render pass may be disabled, or not render for the current view (scene/game view not open)", MessageType.Info);
                return;
            }
            
            EditorGUILayout.LabelField($"\"{renderTarget.textureName}\" {renderTarget.rt.graphicsFormat} {renderTarget.rt.width}x{renderTarget.rt.height}px @ {(UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(renderTarget.rt) / 1024f / 1024f).ToString("F2")}mb", EditorStyles.boldLabel);
            if(renderTarget.description != string.Empty) EditorGUILayout.HelpBox(renderTarget.description, MessageType.Info);
            
            Rect rect = EditorGUILayout.GetControlRect();

            Rect position = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);
            position.width = width;

            //colorChannel = EditorGUI.Popup(position, "Channel mask", colorChannel, new string[] { "RGB", "R", "G", "B", "A" }); 
            colorChannel = (int)GUI.Toolbar(position, colorChannel, new GUIContent[] { new GUIContent("RGBA"), new GUIContent("RGB"), new GUIContent("R"), new GUIContent("G"), new GUIContent("B"), new GUIContent("A") });

            switch (colorChannel)
            {
                case 1: colorMask = ColorWriteMask.All;
                    break;
                case 2: colorMask = ColorWriteMask.Red;
                    break;
                case 3: colorMask = ColorWriteMask.Green;
                    break;
                case 4: colorMask = ColorWriteMask.Blue;
                    break;
                case 5: colorMask = ColorWriteMask.Alpha;
                    break;
            }

            rect.y += 21f;
            rect.width = width;
            float aspect = (renderTarget.rt.height / renderTarget.rt.width);
            rect.height = rect.width;

            if (colorChannel == 0) //RGBA
            {
                EditorGUI.DrawTextureTransparent(rect, renderTarget.rt, ScaleMode.ScaleToFit, aspect);
            }
            else if (colorMask == ColorWriteMask.Alpha)
            {
                EditorGUI.DrawTextureAlpha(rect, renderTarget.rt, ScaleMode.ScaleToFit, aspect, 0);
            }
            else
            {
                EditorGUI.DrawPreviewTexture(rect, renderTarget.rt, null, ScaleMode.ScaleToFit, aspect, 0, colorMask, exposure);
            }
            GUILayout.Space(rect.height);
            exposure = EditorGUILayout.Slider("Exposure", exposure, 1f, 16f);
        }
    }
}