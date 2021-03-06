using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private ScreenBase[] screens;
    private GameState gameState;

    private string sceneToUnload;

    private void Start()
    {
        App.screenManager = this;
        screens = GetComponentsInChildren<ScreenBase>(true);
        Show<MenuScreen>();
    }

    private void Update()
    {
        //PauseMenuScreenSwitch();
    }

    public void Show<T>()
    {
        foreach(var screen in screens)
        {
            if(screen.GetType() == typeof(T))
            {
                screen.Show();
                break;
            }
        }
    }

    public void Hide<T>()
    {
        foreach (var screen in screens)
        {
            if (screen.GetType() == typeof(T))
            {
                screen.Hide();
                break;
            }
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public bool CompareGameState(GameState gameState)
    {
        return gameState == this.gameState;
    }

    /*
    private void PauseMenuScreenSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.running)
                Show<PauseMenuScreen>();
            else if (gameState == GameState.paused)
                Hide<PauseMenuScreen>();
        }
    }
    */

    //Funkcia pre button v PauseMenuScreene
    public void HidePauseScreen()
    {
        Hide<PauseMenuScreen>();
    }

    public void SetSceneToUnload(string scene)
    {
        sceneToUnload = scene;
    }

    public string GetSceneToUnload()
    {
        return sceneToUnload;
    }
}