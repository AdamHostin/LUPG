using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreen : ScreenBase
{
    public PlayerAvatar[] players = new PlayerAvatar[4];

    bool canChoose = false;

    private void Awake()
    {
        App.lobbyScreen = this;
    }

    public override void Show()
    {
        base.Show();
        ResetAvatars();
        App.inputManager.EnableJoining();
        canChoose = true;
    }

    public override void Hide()
    {
        canChoose = false;
        App.playerManager.DeletePlayers();
        App.inputManager.DisableJoining();
        base.Hide();
    }

    public void BackButtonClicked()
    {
        Hide();
        App.screenManager.Show<MenuScreen>();
    }

    public PlayerAvatar GetAvatar()
    {
        foreach (PlayerAvatar player in players)
        {
            if (!player.IsOccupied())
            {
                player.SetOccupation();
                return player;
            }
        }

        //Say that all places are occupied
        return null;
    }

    void ResetAvatars()
    {
        foreach (PlayerAvatar player in players)
        {
            player.DeleteOccupation();
        }
    }

    public bool CanChoose()
    {
        return canChoose;
    }
}