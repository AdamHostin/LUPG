using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : ScreenBase
{
    [SerializeField] GameObject continnueButton;

    public override void Show()
    {
        App.screenManager.SetGameState(GameState.menu);
        App.audioManager.PlayLoop("MenuMusic");
        base.Show();
        if (App.gameManager.GetSceneIndex() != 0) continnueButton.SetActive(true);
        else continnueButton.SetActive(false);
    }

    public override void Hide()
    {
        //App.audioManager.Stop("MenuSound");
        base.Hide();
    }

    public void PlayButtonClicked()
    {
        App.screenManager.Hide<MenuScreen>();
        App.screenManager.Show<LobbyScreen>();
        //StartCoroutine(App.audioManager.PlayAmbient());
        //App.audioManager.Play("UIButtonClicked");
    }

    public void TestLevelButtonClicked()
    {
        //App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartSceneLoading("TestLevel");
        App.screenManager.Hide<MenuScreen>();
        Time.timeScale = 1;
        //StartCoroutine(App.audioManager.PlayAmbient());
    }

    public void CointinueButtonClicked()
    {
        //App.audioManager.Play("UIButtonClicked");
        App.gameManager.StartCurrentSceneLoading();
        Time.timeScale = 1;
        //StartCoroutine(App.audioManager.PlayAmbient());
    }

    public void SettingsButtonClicked()
    {
        App.screenManager.Show<SettingsScreen>();
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}