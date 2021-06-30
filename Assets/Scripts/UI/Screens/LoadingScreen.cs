using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LoadingScreen : ScreenBase
{
    [SerializeField] string sceneToLoad;
    [SerializeField] float timeToStart;

    [SerializeField] VideoClip loadingClip;
    [SerializeField] VideoClip countdownClip;

    [Header("Don't touch")]
    [SerializeField] Button button;

    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
    }

    public override void Show()
    {
        base.Show();
        videoPlayer.isLooping = true;
        videoPlayer.clip = loadingClip;
        videoPlayer.Play();
    }

    public void StartButtonClicked()
    {
        button.enabled = false;
        videoPlayer.isLooping = false;
        videoPlayer.clip = countdownClip;
        videoPlayer.Play();
        Invoke("StartGame", timeToStart);
    }

    void StartGame()
    {
        button.enabled = true;
        App.screenManager.Show<InGameScreen>();
        App.gameManager.StartSceneLoading(sceneToLoad);
        App.screenManager.SetGameState(GameState.running);
        Hide();
    }
}