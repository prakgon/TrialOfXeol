using UnityEngine;

namespace TOX.Audio
{
    [CreateAssetMenu(fileName = "AudioManager", menuName = "TOX/Audio/AudioManager", order = 1)]
    public class AudioControllerData : ScriptableObject
    {
        public float volume = 1f;   
        public bool mute = false;
    }
}