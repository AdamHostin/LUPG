using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadingScreen : ScreenBase
{
    [SerializeField] string sceneToLoad;
    [SerializeField] float timeToStart;

    [SerializeField] VideoClip loadingClip;
    [SerializeField] VideoClip countdownClip;

    VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    public override void Show()
    {
        base.Show();
        //videoPlayer.clip = loadingClip;
        //videoPlayer.Play();
    }

    public void StartButtonClicked()
    {
        //videoPlayer.clip = countdownClip;
        //videoPlayer.Play();
        Invoke("StartGame", timeToStart);
    }

    void StartGame()
    {
        App.screenManager.Show<InGameScreen>();
        App.gameManager.StartSceneLoading(sceneToLoad);
        App.screenManager.SetGameState(GameState.running);
        Hide();
    }
}