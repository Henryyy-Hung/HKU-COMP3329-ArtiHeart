using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 这个命名空间提供了对UI组件的访问

public class SceneLoader : MonoBehaviour
{
    private string sceneToLoad;

    // 调用这个方法来设置想要加载的场景名称
    public void SetSceneToLoad(string sceneName)
    {
        sceneToLoad = sceneName;
    }

    // 绑定到按钮的OnClick()事件
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
