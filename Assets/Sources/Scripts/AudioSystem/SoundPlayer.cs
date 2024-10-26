using UnityEngine;

namespace AudioSystem
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundData soundData;
        [SerializeField] private Vector3 soundPosition;
        [SerializeField] private bool _playSoundInUpdate = false;
        [SerializeField] private float _playInterval = 0.1f;

        private float _timeSinceLastPlay = 0f;

        private void Update()
        {
            if (_playSoundInUpdate)
            {
                _timeSinceLastPlay += Time.deltaTime;

                if (_timeSinceLastPlay >= _playInterval)
                {
                    PlaySound();
                    _timeSinceLastPlay = 0f;
                }
            }
        }

        public void PlaySound()
        {
            if (SoundManager.Instance != null && soundData != null)
            {
                SoundBuilder soundBuilder = SoundManager.Instance.CreateSoundBuilder();
                soundBuilder.WithPosition(soundPosition)
                             .WithRandomPitch()
                             .Play(soundData);
            }
            else
            {
                Debug.LogWarning("SoundManager ��� SoundData �� �����������!");
            }
        }

        [ContextMenu("Play Sound")]
        public void PlaySoundFromInspector()
        {
            PlaySound();
        }
    }
}