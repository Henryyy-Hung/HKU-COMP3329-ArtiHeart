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
            // ע����Ƶ������ϵ��¼�
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
        // ���� RawImage �� GameObject
        Destroy(rawImage.gameObject);
    }
}