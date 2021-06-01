using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarController : MonoBehaviour
{
    PlayerAvatar playerAvatar;
    int avatarIndex;

    private void Start()
    {
        playerAvatar = App.lobbyScreen.GetAvatar(this);
        if (playerAvatar == null)
            Destroy(gameObject);
    }

    public void ChooseCharacter(InputAction.CallbackContext context)
    {
        if (!App.screenManager.CompareGameState(GameState.lobby) || !context.performed)
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
        }
    }

    public void SetAvatarIndex(int index)
    {
        avatarIndex = index;
    }
}