using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ARSceneManager : MonoBehaviour
{
    private Scene MyScene;
    public void GotoMainScene()
    {
        MyScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(MyScene);
    }

    public void GotoScene(string sceneName)
    {
        MyScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(MyScene);
    }

    int ClickCount = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickCount++;
        }
        else if (ClickCount == 2)
        {
            CancelInvoke("DoubleClick");
            Application.Quit();
        }
    }

    public void gotosoma()
    {
        SceneManager.LoadScene("soma");
    }
    public void gotostudy()
    {
        SceneManager.LoadScene("Study-3");
    }
    public void gotofree()
    {
        SceneManager.LoadScene("Study-2");
    }

}