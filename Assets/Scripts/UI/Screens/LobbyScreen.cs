using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScreen : ScreenBase
{
    public PlayerAvatar[] players = new PlayerAvatar[4];
    [SerializeField] string sceneToLoad;

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
        App.screenManager.SetGameState(GameState.lobby);
    }

    public override void Hide()
    {
        canChoose = false;
        App.inputManager.DisableJoining();
        base.Hide();
    }

    public void StartButtonClicked()
    {
        bool canStart = true;
        foreach (PlayerAvatar player in players)
        {
            if (player.IsOccupied() && !player.IsReady())
                canStart = false;
        }
        
        if (canStart)
        {
            SendIndexes();
            Hide();
            App.gameManager.StartSceneLoading(sceneToLoad);
            App.screenManager.SetGameState(GameState.running);
        }
        else
        {
            Debug.LogWarning("Lobby empty");
        }
    }

    public void BackButtonClicked()
    {
        App.playerManager.DeletePlayers();
        Hide();
        App.screenManager.Show<MenuScreen>();
    }

    public PlayerAvatar GetAvatar(AvatarController avatarController)
    {
        foreach (PlayerAvatar player in players)
        {
            if (!player.IsOccupied())
            {
                player.SetOccupation(avatarController);
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

    void SendIndexes()
    {
        foreach (PlayerAvatar player in players)
        {
            if (player.IsOccupied())
            {
                player.SendAvatarIndex();
            }
        }
    }
}