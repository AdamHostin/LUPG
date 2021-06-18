using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarController : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerAvatar playerAvatar;
    int avatarIndex;

    bool canChoose = true;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
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
            playerHealth.SetAvatar(playerAvatar.GetCurrentAvatar());
        }
    }

    public void SetAvatarIndex(int index)
    {
        avatarIndex = index;
    }
}