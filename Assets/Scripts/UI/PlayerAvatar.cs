using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAvatar : MonoBehaviour
{
    private Image image;

    private bool isOccupied = false;
    private int pictureIndex = 0;
    Sprite[] sprites;

    [SerializeField] Sprite idleSprite;

    bool isReady = false;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        sprites = App.playerManager.GetPlayerAvatars();
        ResetImage();
    }

    public void SetOccupation()
    {
        isOccupied = true;
        image.sprite = sprites[pictureIndex];
    }

    public void DeleteOccupation()
    {
        isOccupied = false;
        ResetImage();
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void IncrementImage()
    {
        Debug.Log("increment");
        pictureIndex++;
        if (pictureIndex == sprites.Length)
            pictureIndex = 0;
        image.sprite = sprites[pictureIndex];
    }

    public void DecrementImage()
    {
        Debug.Log("decrement");
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

    public void ToggleReady()
    {
        isReady = !isReady;
    }
}