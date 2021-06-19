using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionDecider : MonoBehaviour
{
    CharacterController2D characterController;
    AvatarController avatarController;

    private void Start()
    {
        characterController = GetComponent<CharacterController2D>();
        avatarController = GetComponent<AvatarController>();

    }

    public void Move(InputAction.CallbackContext context)
    {
        if (App.screenManager.CompareGameState(GameState.lobby))
            avatarController.ChooseCharacter(context);
        if (App.screenManager.CompareGameState(GameState.running))
            characterController.Move(context);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (App.screenManager.CompareGameState(GameState.lobby))
            avatarController.ReadyUp(context);
        if (App.screenManager.CompareGameState(GameState.running))
            characterController.ManageJump(context);
    }

    public void ChooseCharacter(InputAction.CallbackContext context)
    {
        if (App.screenManager.CompareGameState(GameState.lobby))
            avatarController.ChooseCharacter(context);
    }
}