using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] Image image;

    private bool isOccupied = false;
    [SerializeField] private int pictureIndex = 0;
    Sprite[] sprites;

    [SerializeField] Sprite idleSprite;

    bool isReady = false;
    AvatarController avatarController;

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

    public void SetOccupation(AvatarController avatarController)
    {
        isOccupied = true;
        
        while (App.lobbyScreen.IsSpriteInUse(sprites[pictureIndex]))
        {
            pictureIndex++;
            if (pictureIndex == sprites.Length)
                pictureIndex = 0;
        }
        image.sprite = sprites[pictureIndex];
        this.avatarController = avatarController;

        
    }

    public void DeleteOccupation()
    {
        isOccupied = false;
        avatarController = null;
        SetReady(false);
        ResetImage();
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void IncrementImage()
    {
        do
        {
            pictureIndex++;
            if (pictureIndex == sprites.Length)
                pictureIndex = 0;
        } while (App.lobbyScreen.IsSpriteInUse(sprites[pictureIndex]));
        image.sprite = sprites[pictureIndex];
    }

    public void DecrementImage()
    {
        do
        {
            pictureIndex--;
            if (pictureIndex < 0)
                pictureIndex = sprites.Length - 1;
        } while (App.lobbyScreen.IsSpriteInUse(sprites[pictureIndex]));
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
        if (App.lobbyScreen.IsSpriteInUse(image.sprite) && !isReady) return;

        isReady = !isReady;
        if (isReady) App.lobbyScreen.AddSpriteInUse(image.sprite);
        else App.lobbyScreen.RemoveSpriteInUse(image.sprite);
        checkBox.SetReady(isReady);
    }

    public void SendAvatarIndex()
    {
        avatarController.SetAvatarIndex(pictureIndex);
    }

    public Sprite GetCurrentAvatar()
    {
        return sprites[pictureIndex];
    }

    public int GetPictureIndex()
    {
        return pictureIndex;
    }
}