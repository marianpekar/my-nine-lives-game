using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{ 
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int mainMenuSceneIndex = 0;
    public int gameSceneIndex = 1;

    private int currentSceneIndex;
    public void LoadGameScene() {
        currentSceneIndex = gameSceneIndex;
        Cursor.visible = false;
        PlayerStates.Singleton.Reset();
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void LoadMainMenuScene()
    {
        PlayerStates.Singleton.Reset();
        SceneManager.LoadScene(mainMenuSceneIndex);
        Cursor.visible = true;
        PlayerEvents.Singleton.ClearAllActionLists(); 
        EnvironmentEvents.Singleton.ClearAllActionLists();
        DestroyImmediate(this.gameObject);
    }

    private bool isLoadMainMenuSceneInvoked;
    void FixedUpdate()
    {
        if (GameInputManager.GetKey("Cancel") && currentSceneIndex == gameSceneIndex)
        {
            LoadMainMenuScene();
        }

        if (PlayerStates.Singleton.IsDead && !isLoadMainMenuSceneInvoked)
        {
            Invoke("LoadMainMenuScene", 10 * Time.timeScale);
            isLoadMainMenuSceneInvoked = true;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
