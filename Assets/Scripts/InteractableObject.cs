using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 3f; // 玩家与物体表面的交互距离
    public string sceneToLoad = "YourSceneName"; // 要加载的场景名称
    private GameObject player;
    private bool isPlayerClose = false;
    private Collider objectCollider; // 物体的Collider
    private GUIStyle guiStyle = new GUIStyle();
    private GUIStyle backgroundStyle = new GUIStyle(); // 用于背景框的样式

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 确保玩家有"Player"标签
        objectCollider = GetComponent<Collider>(); // 获取物体的Collider组件

        // 设置提示文本的GUI样式
        guiStyle.fontSize = 24;
        guiStyle.normal.textColor = Color.white;
        guiStyle.alignment = TextAnchor.MiddleCenter; // 设置文本居中对齐

        // 设置背景框的样式
        backgroundStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
    }

    void Update()
    {
        int anger = PlayerPrefs.GetInt("anger");
        int sadness = PlayerPrefs.GetInt("sadness");
        int joy = PlayerPrefs.GetInt("joy");
        int fear = PlayerPrefs.GetInt("fear");

        if (anger == 1 && sceneToLoad == "l4level1")
        {
            Destroy(gameObject);
        }
        else if (sadness == 1 && sceneToLoad == "sadness_main")
        {
            Destroy(gameObject);
        }
        else if (joy == 1 && sceneToLoad == "JoyScene")
        {
            Destroy(gameObject);
        }
        else if (fear == 1 && sceneToLoad == "L2gamestart")
        {
            Destroy(gameObject);

        }

        // 计算玩家与物体表面的最近点
        Vector3 closestPoint = objectCollider.ClosestPoint(player.transform.position);
        // 计算玩家位置和最近点的距离
        float distanceToSurface = Vector3.Distance(player.transform.position, closestPoint);

        // 如果玩家与物体表面的距离小于或等于交互距离，则认为玩家靠近
        isPlayerClose = distanceToSurface <= interactionDistance;

        // 如果玩家靠近物体并按下F键，则加载场景
        if (isPlayerClose && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(sceneToLoad);
            PlayerPrefs.SetInt("inGame", 1);
        }
    }

    void OnGUI()
    {
        if (isPlayerClose)
        {
            // 计算背景框的位置和大小
            Rect boxRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 25, 400, 50);
            // 绘制背景框
            GUI.Box(boxRect, GUIContent.none, backgroundStyle);
            // 使用自定义GUIStyle显示提示信息
            GUI.Label(boxRect, "[F] To Enter Dreamscape", guiStyle);
        }
    }

    // 生成一个纯色的Texture
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}