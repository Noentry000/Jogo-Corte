using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class CutsceneController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(0); // first scene in the build profile's scene list
    }
}