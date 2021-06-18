using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarController : MonoBehaviour
{
    PlayerAvatar playerAvatar;
    int avatarIndex;

    bool canChoose = true;

    private void Start()
    {
        playerAvatar = App.lobbyScreen.GetAvatar(this);
        if (playerAvatar == null)
            Destroy(gameObject);
    }

    public void ChooseCharacter(InputAction.CallbackContext context)
    {
        if (!App.screenManager.CompareGameState(GameState.lobby) || !context.performed || !canChoose)
            return;

        if (App.lobbyScreen.CanChoose())
        {
            if ((int)context.ReadValue<float>() > 0)
            {
                playerAvatar.IncrementImage();
            }
            else
            {
                playerAvatar.DecrementImage();
            }
        }
    }

    public void ReadyUp(InputAction.CallbackContext context)
    {
        if (!App.screenManager.CompareGameState(GameState.lobby))
            return;

        if (App.lobbyScreen.CanChoose())
        {
            playerAvatar.ToggleReady();
            canChoose = !canChoose;
        }
    }

    public void SetAvatarIndex(int index)
    {
        avatarIndex = index;
    }
}