using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void Hover()
    {
        App.audioManager.Play("ButtonHover");
    }

    public void Clicked()
    {
        App.audioManager.Play("ButtonClicked");
    }
}