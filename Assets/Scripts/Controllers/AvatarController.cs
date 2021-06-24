using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarController : MonoBehaviour
{
    PlayerHealth playerHealth;
    PlayerAvatar playerAvatar;
    [SerializeField] GameObject[] characterArray;
    int avatarIndex;

    bool canChoose = true;
    bool hasMoved = false;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerAvatar = App.lobbyScreen.GetAvatar(this);
        if (playerAvatar == null)
            Destroy(gameObject);
    }

    public void ChooseCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed || !canChoose || context.started)
            return;

        int value = Mathf.RoundToInt(context.ReadValue<float>() / 2f);
        
        if (value == 0)
        {
            hasMoved = false;
        }

        Debug.Log(hasMoved);
        if (hasMoved)
            return;

        if (value > 0)
        {
            playerAvatar.IncrementImage();
        }
        else
        {
            playerAvatar.DecrementImage();
        }

        hasMoved = true;
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
            SetCharacter(playerAvatar.GetPictureIndex());
        }
    }

    public void SetAvatarIndex(int index)
    {
        avatarIndex = index;
    }

    public  void SetCharacter(int idx)
    {
        foreach (var character in characterArray)
        {
            character.SetActive(false);
        }
        characterArray[idx].SetActive(true);
    }
}