using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int gameSceneIndex = 0;
    public void LoadGameScene() {
        SceneManager.LoadScene(gameSceneIndex);
    }
    
}
