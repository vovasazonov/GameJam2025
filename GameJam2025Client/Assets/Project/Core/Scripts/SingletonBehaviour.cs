using UnityEngine;

namespace Project.Core.Scripts
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T Instance { get; protected set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new System.Exception($"An instance of this singleton already exists: {typeof(T).Name}");
            }
            else
            {
                Instance = (T)this;
            }
        }
    }
}