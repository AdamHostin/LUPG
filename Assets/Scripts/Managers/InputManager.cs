using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    private void Awake()
    {
        App.inputManager = this;
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void DisableJoining()
    {
        playerInputManager.DisableJoining();
    }

    public void EnableJoining()
    {
        playerInputManager.EnableJoining();
    }
}