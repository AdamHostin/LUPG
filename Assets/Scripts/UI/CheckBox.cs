using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    [Header("0 => unready")]
    [Header("1 => ready")]
    [SerializeField] Sprite[] sprites = new Sprite[2];
    bool isReady;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.sprite = sprites[0];
    }

    public void SetReady(bool value)
    {
        if (value)
            image.sprite = sprites[1];
        else
            image.sprite = sprites[0];
    }
}