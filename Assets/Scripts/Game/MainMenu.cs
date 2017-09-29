using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //REFAZER ESSA CLASSE

    private void Awake()
    {
        Cursor.visible = true;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void goToScene(string sceneName)
    {
        StartCoroutine(this.Timer(sceneName));
    }

    private IEnumerator Timer(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        Cursor.visible = true;
    }

}
