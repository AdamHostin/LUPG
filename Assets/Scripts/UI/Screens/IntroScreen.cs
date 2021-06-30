using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : ScreenBase
{

    public void StartButtonClicked()
    {
        App.screenManager.Hide<IntroScreen>();
        App.screenManager.Show<MenuScreen>();
    }
}
