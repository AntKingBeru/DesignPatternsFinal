// Generic MonoBehaviour singleton base. Guarantees one instance and global access. Reused by both games.
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // Global access point to the singleton instance.
    public static T Instance { get; private set; }
    
    [SerializeField] private bool persistAcrossScenes;

    protected virtual void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = (T)this;
        
        if (persistAcrossScenes)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        // Release the reference so a reloaded scene can register a fresh instance.
        if (Instance == this)
            Instance = null;
    }
}