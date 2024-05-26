using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ��������ռ��ṩ�˶�UI����ķ���

public class SceneLoader : MonoBehaviour
{
    private string sceneToLoad;

    // �������������������Ҫ���صĳ�������
    public void SetSceneToLoad(string sceneName)
    {
        sceneToLoad = sceneName;
    }

    // �󶨵���ť��OnClick()�¼�
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
