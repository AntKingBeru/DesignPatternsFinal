// Central scene loading; always restores normal timescale so a paused game doesn't load frozen.
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Load a scene by name, ensuring the game is unpaused first.
    public static void Load(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}