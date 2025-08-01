using Project.Core.Scripts;

namespace Project.Features.Audio.Scripts
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        public bool IsOn { get; set; }
    }
}