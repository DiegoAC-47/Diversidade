using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    [SerializeField]
    private CutScene startCutScene;

    private bool startcgPlayed;
    private bool paused, inDialog;
    public event Action Pause, UnPause;
    public event Predicate<bool> DialogStatusChanged;

    private void Awake()
    {
        Cursor.visible = false;

        if (!this.GetComponent<GlobalOptions>())
            this.gameObject.AddComponent<GlobalOptions>();
        if (!this.GetComponent<CutSceneSystem>())
            this.gameObject.AddComponent<CutSceneSystem>();

        Application.targetFrameRate = this.GetComponent<GlobalOptions>().TargetFrameRate;
    }

    private void Start()
    {
        FindObjectOfType<SaveManager>().Load();
    }

    private void Update ()
    {
        if(!this.startcgPlayed)
        {
            this.startcgPlayed = true;
            if(this.startCutScene != null && this.GetComponent<GlobalOptions>().PlayStartCutScene)
                this.startCutScene.StartCinematic();
        }
	}

    public bool Paused
    {
        get
        {
            return paused;
        }

        set
        {
            this.paused = value;
            AudioListener.pause = value;
            if (value)
                this.OnPause();
            else
                this.OnUnPause();
        }
    }

    public bool InDialog
    {
        get
        {
            return this.inDialog;
        }

        set
        {
            this.inDialog = value;
            this.paused = value;
            if (this.DialogStatusChanged != null)
                this.DialogStatusChanged(this.InDialog);
        }
    }

    public bool StartcgPlayed
    {
        get
        {
            return startcgPlayed;
        }

        set
        {
            startcgPlayed = value;
        }
    }

    private void OnPause()
    {
        if (this.Pause != null)
            this.Pause();
    }

    private void OnUnPause()
    {
        if (this.UnPause != null)
            this.UnPause();
    }

    public void TimePause(bool value)
    {
        if (value)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void Coninue()
    {
        FindObjectOfType<InterfaceManager>().MenuOnOff();
    }

    public void LoadScene(string sceneName)
    {
        this.TimePause(false);
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        this.TimePause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
