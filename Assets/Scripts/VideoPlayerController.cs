using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    void Start()
    {
        int videoPlayed = PlayerPrefs.GetInt("videoPlayed");

        if (videoPlayed != 1)
        {
            // 注册视频播放完毕的事件
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
            PlayerPrefs.SetInt("videoPlayed", 1);
        }
        else
        {
            Destroy(rawImage.gameObject);
        }
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        // 销毁 RawImage 的 GameObject
        Destroy(rawImage.gameObject);
    }
}