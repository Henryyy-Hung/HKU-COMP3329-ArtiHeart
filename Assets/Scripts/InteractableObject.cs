using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 3f; // ������������Ľ�������
    public string sceneToLoad = "YourSceneName"; // Ҫ���صĳ�������
    private GameObject player;
    private bool isPlayerClose = false;
    private Collider objectCollider; // �����Collider
    private GUIStyle guiStyle = new GUIStyle();
    private GUIStyle backgroundStyle = new GUIStyle(); // ���ڱ��������ʽ

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // ȷ�������"Player"��ǩ
        objectCollider = GetComponent<Collider>(); // ��ȡ�����Collider���

        // ������ʾ�ı���GUI��ʽ
        guiStyle.fontSize = 24;
        guiStyle.normal.textColor = Color.white;
        guiStyle.alignment = TextAnchor.MiddleCenter; // �����ı����ж���

        // ���ñ��������ʽ
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

        // ����������������������
        Vector3 closestPoint = objectCollider.ClosestPoint(player.transform.position);
        // �������λ�ú������ľ���
        float distanceToSurface = Vector3.Distance(player.transform.position, closestPoint);

        // ���������������ľ���С�ڻ���ڽ������룬����Ϊ��ҿ���
        isPlayerClose = distanceToSurface <= interactionDistance;

        // �����ҿ������岢����F��������س���
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
            // ���㱳�����λ�úʹ�С
            Rect boxRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 25, 400, 50);
            // ���Ʊ�����
            GUI.Box(boxRect, GUIContent.none, backgroundStyle);
            // ʹ���Զ���GUIStyle��ʾ��ʾ��Ϣ
            GUI.Label(boxRect, "[F] To Enter Dreamscape", guiStyle);
        }
    }

    // ����һ����ɫ��Texture
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