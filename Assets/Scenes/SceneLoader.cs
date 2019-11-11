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
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
        DestroyImmediate(this.gameObject);
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Escape) && currentSceneIndex == gameSceneIndex)
            LoadMainMenuScene();
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
