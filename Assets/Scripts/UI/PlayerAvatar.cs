using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] Image image;

    private bool isOccupied = false;
    private int pictureIndex = 0;
    Sprite[] sprites;

    [SerializeField] Sprite idleSprite;

    bool isReady = false;
    CharacterController2D playerController;

    CheckBox checkBox;

    private void Awake()
    {
        checkBox = GetComponentInChildren<CheckBox>();
    }

    private void Start()
    {
        sprites = App.playerManager.GetPlayerAvatars();
        ResetImage();
    }

    public void SetOccupation(CharacterController2D playerController)
    {
        isOccupied = true;
        image.sprite = sprites[pictureIndex];
        this.playerController = playerController;
    }

    public void DeleteOccupation()
    {
        isOccupied = false;
        playerController = null;
        SetReady(false);
        ResetImage();
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void IncrementImage()
    {
        pictureIndex++;
        if (pictureIndex == sprites.Length)
            pictureIndex = 0;
        image.sprite = sprites[pictureIndex];
    }

    public void DecrementImage()
    {
        pictureIndex--;
        if (pictureIndex < 0)
            pictureIndex = sprites.Length - 1;
        image.sprite = sprites[pictureIndex];
    }

    public void ResetImage()
    {
        pictureIndex = 0;
        image.sprite = idleSprite;
    }

    public bool IsReady()
    {
        return isReady;
    }

    public void SetReady(bool value)
    {
        isReady = value;
        checkBox.SetReady(isReady);
    }

    public void ToggleReady()
    {
        isReady = !isReady;
        checkBox.SetReady(isReady);
    }

    public void SendAvatarIndex()
    {
        playerController.SetAvatarIndex(pictureIndex);
    }
}