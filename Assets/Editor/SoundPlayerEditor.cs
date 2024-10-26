using AudioSystem;
using UnityEditor;
using UnityEngine;

namespace AudioSystem
{
    [CustomEditor(typeof(AudioSystem.SoundPlayer))]
    public class SoundPlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SoundPlayer soundPlayer = (SoundPlayer)target;

            // ��������� ������ � ���������
            if (GUILayout.Button("Play Sound"))
            {
                soundPlayer.PlaySoundFromInspector();
            }
        }
    }
}