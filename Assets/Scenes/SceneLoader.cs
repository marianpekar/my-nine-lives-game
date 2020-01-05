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
        PlayerStates.Singleton.Reset();
        StartCoroutine(LoadSceneAsync(gameSceneIndex));
    }

    public void LoadMainMenuScene()
    {
        PlayerStates.Singleton.Reset();
        SceneManager.LoadScene(mainMenuSceneIndex);
        PlayerEvents.Singleton.ClearAllActionLists(); 
        DestroyImmediate(this.gameObject);
    }

    void LateUpdate()
    {
        if (GameInputManager.GetKey("Cancel") && currentSceneIndex == gameSceneIndex)
        {
            LoadMainMenuScene();
        }
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Level loading:" + progress);

            yield return null;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
