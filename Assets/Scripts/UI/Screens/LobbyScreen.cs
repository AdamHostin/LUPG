using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyScreen : ScreenBase
{
    public PlayerAvatar[] players = new PlayerAvatar[4];
    [SerializeField] string sceneToLoad;

    [SerializeField] List<Sprite> spritesInUse = new List<Sprite>();

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
        List<int> indexes = new List<int>();
        foreach (PlayerAvatar player in players)
        {
            if (player.IsOccupied() && !player.IsReady())
                return;
            if (player.IsOccupied())
                indexes.Add(player.GetPictureIndex());
        }

        if (indexes.Count != indexes.Distinct().Count())
        {
            return;
        }

        SendIndexes();
        Hide();
        App.screenManager.Show<InGameScreen>();
        App.gameManager.StartSceneLoading(sceneToLoad);
        App.screenManager.SetGameState(GameState.running);
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

    public void AddSpriteInUse(Sprite sprite)
    {
        spritesInUse.Add(sprite);
    }

    public void RemoveSpriteInUse(Sprite sprite)
    {
        spritesInUse.Remove(sprite);
    }

    public bool IsSpriteInUse(Sprite sprite)
    {
        return spritesInUse.Contains(sprite);
    }
}