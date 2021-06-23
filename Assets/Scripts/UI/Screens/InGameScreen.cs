using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScreen : ScreenBase
{
    [SerializeField] BarController[] barControllers; 

    public void ReturnToMenu()
    {
        App.gameManager.StartSceneUnloading("Level1");
    }

    public override void Show()
    {
        App.screenManager.SetGameState(GameState.running);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        //App.CameraManager.DisableCamera();

    }

    public void PauseButtonClicked()
    {
        App.screenManager.Show<PauseMenuScreen>();
    }

    private void OnEnable()
    {
        int playerCount = App.playerManager.GetPlayerCount();
        for (int i = 0; i < playerCount; i++)
        {
            barControllers[i].gameObject.SetActive(true);
            App.playerManager.players[i].SetHpBar(barControllers[i]);
        }
    }

    private void OnDisable()
    {
        foreach (var item in barControllers)
        {
            item.gameObject.SetActive(false);
        }
    }
}