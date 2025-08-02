using Project.Core.Scripts;
using UnityEngine;

namespace Project.Features.Audio.Scripts
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        private const string MUSIC_ON = "MUSIC_ON";
        public bool IsOn { get => PlayerPrefs.GetInt(MUSIC_ON, 1) == 1; set => PlayerPrefs.SetInt(MUSIC_ON, value ? 1 : 0); }
    }
}