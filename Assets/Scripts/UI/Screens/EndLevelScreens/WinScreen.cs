using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : EndLevelScreen
{
    [SerializeField] Image[] images;
    public override void Show()
    {
        base.Show();
        int idx = 0;
        Debug.Log(App.playerManager.playerOrder.Count);
        while (App.playerManager.playerOrder.Count!=0 || idx<2)
        {   
            images[idx].sprite = App.playerManager.playerOrder.Dequeue();
            idx++;

        }
        App.playerManager.playerOrder.Clear();
    }
}