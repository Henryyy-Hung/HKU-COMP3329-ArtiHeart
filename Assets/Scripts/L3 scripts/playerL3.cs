using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerL3 : MonoBehaviour
{
    public float maxPlainarSpeed = 3f;  // The maximum moving speed on a plainar direction (the X - Z plain, for example)
    public float rotateSpeed = 1000f;   // The rotation speed when turning around towards the current speed direction

    private string game_state = "init";
    public string MainScene, ThisScene;
    public GameObject Chest;
    public AudioClip open_chest, bgm1, bgm2;
    public AudioSource audioSource;
    private ChestL3 chest_script;

    // parameters for camera
    private Vector3 offset = new Vector3(0f, 2f, -3f);
    private float min_offset = 1.5f;
    private float max_offset = 7f;
    private float MouseRotateSpeed = 20f;
    private float MouseScrollSpeed = 1f;

    // parameters for character
    private float gravity = -0.2f;
    private CharacterController chara;
    private float jumpSpeed = 5.5f;    // The jumping speed when pressing 'space' button
    private float moveSpeed = 4f;
    private Vector3 moveDirection, orien_z, orien_y, orien_x;
    private float verticalSpeed;
    private bool isJump, isSelecting, isHorizontalStatic;
    private Animator animator;

    // parameters for collision
    private List<GameObject> hit_blocks;
    private string current_block_tag;
    private GameObject target_block;

    // parameters for terrain
    public int tutorial_terrain_radius, level_terrain_radius;
    public GameObject sample_cube;  // terrain_prefab
    private int terrain_radius;
    private Renderer block_renderer;
    private Material block_material;
    public Cubemap sunnyCubeMap;
    private int block_num;
    private List<(UnityEngine.Color, string)> block_colors = new List<(UnityEngine.Color, string)>() { (new Color32(105, 100, 123, 255), "typeA"), (new Color32(173, 174, 178, 255), "typeB"), (new Color32(165, 108, 65, 255), "typeC") };
    private List<GameObject> remaining_blocks = new List<GameObject>();
    private bool game_clear = false, lastfalling = false;

    // parameters for UI
    public GameObject GuidancePanel, MenuPanel, PuzzlePanel, SelectionBar, PuzzleGuidance, GameClearPrompt, WelcomePanel, TutorialFinishedPanel;
    public Button proceed, exit, resume, restart, guidance;
    private GameObject selection_block_1, selection_block_2, selection_block_3;

    // Start is called before the first frame update
    void Start()
    {
        UI_init();
        //sample_cube = GameObject.Find("sampleCube");
        block_renderer = sample_cube.GetComponent<Renderer>();
        chara = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        chest_script = Chest.GetComponent<ChestL3>();
        WelcomePanel.SetActive(true);
        MouseActive();
        PlayMusic(bgm1);
    }

    // Update is called once per frame
    void Update()
    {
        switch (game_state)
        {
            case "playing":
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnMenuClick();
                    break;
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    game_state = "mousing";
                    MouseActive();
                    break;
                }
                else if (chest_script.puzzling == true)
                {
                    game_state = "puzzle";
                    MouseActive();
                    break;
                }

                if (game_clear == true)
                {
                    Material skyboxMaterial = new Material(Shader.Find("Skybox/Cubemap"));
                    skyboxMaterial.SetTexture("_Tex", sunnyCubeMap);
                    RenderSettings.skybox = skyboxMaterial;
                    PlayMusic(bgm2);
                }
                CharacterMove();
                CameraFunc();
                break;
            case "tutorial":
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnMenuClick();
                    break;
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    game_state = "mousing";
                    MouseActive();
                    break;
                }

                CharacterMove();
                CameraFunc();
                break;
            case "menuing":
                break;
            case "mousing":
                if (Input.GetKeyUp(KeyCode.LeftAlt))
                {
                    game_state = "playing";
                    MouseHidden();
                    break;
                }
                CharacterMove();
                CameraFunc();
                break;
            case "puzzle":
                if (chest_script.puzzling == false)
                {
                    GameClearPrompt.SetActive(true);
                    game_state = "menuing";
                    PlayMusic(open_chest, false);
                    PlayerPrefs.SetInt("sadness", 1);
                    break;
                }
                break;
            case "init":
                break;
        }
    }

    void LateUpdate()
    {
        //Debug.Log(remaining_blocks.Count);
        if (remaining_blocks.Count < 7 && remaining_blocks.Count > 0 && !lastfalling)
        {
            lastfalling = true;
            foreach (GameObject obj in remaining_blocks)
            {
                //remaining_blocks.Remove(obj);
                //Destroy(obj);
                StartCoroutine(BlockFall(obj));
                BlockChoiceInitialize();
                ClearImageColor();
            }
        }
        else if (remaining_blocks.Count == 0 && game_clear)
        {
            if (game_state == "playing")
            {
                GameObject cloud_ocean = GameObject.Find("Cloud ocean");
                StartCoroutine(CloudFall(cloud_ocean));
            }
            else if (game_state == "tutorial")
            {
                game_state = "menuing";
                Time.timeScale = 0f;
                TutorialFinishedPanel.SetActive(true);
                MouseActive();
            }
            
        }
    }

    private void CharacterMove()
    {
        Vector3 previous_position = transform.position;
        orien_z = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        orien_x = (Quaternion.Euler(0f, 90f, 0f) * orien_z);

        float adValue = Input.GetAxis("Horizontal");
        float wsValue = Input.GetAxis("Vertical");

        moveDirection = (orien_z * wsValue) + (orien_x * adValue);

        if (adValue == 0f && wsValue == 0f)
        {
            isHorizontalStatic = true;
        }
        else
        {
            isHorizontalStatic = false;
        }


        if (Input.GetKey(KeyCode.Space) && chara.velocity.y == 0f && isJump == false) // jumping is valid only when on the ground
        {
            verticalSpeed += gravity + jumpSpeed;
            isJump = true;
            if (isHorizontalStatic == true)
            {
                isSelecting = true;
            }
            //Debug.Log("jumped");
        }
        else if (!chara.isGrounded)
        {
            verticalSpeed += gravity;
        }
        else
        {
            verticalSpeed = 0f;
        }

        // first turn to the new direction
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("IsWalking", true);
            chara.transform.rotation = Quaternion.RotateTowards(chara.transform.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);
            //chara.MoveRotation(Quaternion.RotateTowards(chara.rotation, Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime));
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        // then start moving
        Vector3 moving = (moveDirection * moveSpeed + Vector3.up * verticalSpeed) * Time.deltaTime;
        chara.Move(moving);
        Camera.main.transform.position += transform.position - previous_position;

    }

    private void CameraFunc()
    {
        float rotateX = Input.GetAxis("Mouse X") * MouseRotateSpeed * Time.deltaTime;
        float rotateY = Input.GetAxis("Mouse Y") * MouseRotateSpeed * Time.deltaTime;
        float MouseScroll = Input.GetAxis("Mouse ScrollWheel") * MouseScrollSpeed;

        /*if (rotateX == 0f && rotateY == 0f && MouseScroll == 0f)
        {
            return;
        }*/
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector3 camera_orientation = Camera.main.transform.forward;

        orien_z = new Vector3(camera_orientation.x, 0f, camera_orientation.z).normalized;
        orien_y = Vector3.up;
        orien_x = new Vector3(camera_orientation.z, 0f, -camera_orientation.x).normalized;

        // perspective lock
        if (camera_orientation.y > 0.1 && rotateY > 0)
        {
            rotateY = 0f;
        }
        else if (camera_orientation.y > 0.6 && rotateY < 0)
        {
            rotateY = 0f;
        }
        Vector3 direction = (Camera.main.transform.position - transform.position - orien_x * rotateX - orien_y * rotateY).normalized;

        // restriction on the camera-character distance
        if (Vector3.Distance(transform.position, Camera.main.transform.position) > max_offset && MouseScroll < 0)
        {
            MouseScroll = 0;
        }
        else if (Vector3.Distance(transform.position, Camera.main.transform.position) < min_offset && MouseScroll > 0)
        {
            MouseScroll = 0;
        }

        // adjusting camera position
        Camera.main.transform.position = transform.position + direction * distance * (1 - MouseScroll);
        Camera.main.transform.LookAt(chara.transform);
        return;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Cloud" && remaining_blocks.Count != 0 && !lastfalling)
        {
            Respawn();
            Debug.Log("respawned.");
        }
        else if (isJump == true)
        {
            isJump = false;

            if (isSelecting == true && isHorizontalStatic == true)
            {
                target_block = hit.gameObject;
                isSelecting = false;
                isHorizontalStatic = false;

                if (hit_blocks.Count == 0)
                {
                    hit_blocks.Add(target_block);
                    current_block_tag = target_block.tag;
                    //Debug.Log("Hit on block: " + current_block_tag);
                    ImageColoring(selection_block_1, target_block.GetComponent<Renderer>().material.GetColor("_BaseColor"));
                }
                else if (current_block_tag == target_block.tag && !hit_blocks.Contains(target_block))
                {
                    hit_blocks.Add(target_block);
                    //Debug.Log("Hit " + hit_blocks.Count.ToString() + " blocks.");
                    if (hit_blocks.Count == 2)
                    {
                        ImageColoring(selection_block_2, target_block.GetComponent<Renderer>().material.GetColor("_BaseColor"));
                    }
                    else if (hit_blocks.Count == 3)
                    {
                        ImageColoring(selection_block_3, target_block.GetComponent<Renderer>().material.GetColor("_BaseColor"));
                    }
                }
                else if (current_block_tag != target_block.tag)
                {
                    BlockChoiceInitialize();
                    //Debug.Log("Wrong block!");
                    ClearImageColor();
                }

                if (hit_blocks.Count == 3)
                {
                    //Debug.Log("Success! Remaining: " + remaining_blocks.Count.ToString());
                    foreach (GameObject obj in hit_blocks)
                    {
                        StartCoroutine(BlockFall(obj));
                    }
                    BlockChoiceInitialize();
                    ClearImageColor();
                }
            }
        }
    }

    private IEnumerator BlockFall(GameObject obj)
    {
        float flashing_duration = 3.1f;
        float falling_speed = 1.5f;
        float falling_duration = 5f;

        float dtime = 0f;
        float elapsedtime = 0f;
        byte b255 = 255;
        while (elapsedtime < flashing_duration)
        {
            if (dtime > 0.5f)
            {
                Color32 prev_color = obj.GetComponent<Renderer>().material.GetColor("_BaseColor");
                obj.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color32((byte)(b255 - prev_color.r), (byte)(b255 - prev_color.g), (byte)(b255 - prev_color.b), 255));
                dtime = 0f;
            }

            elapsedtime += Time.deltaTime;
            dtime += Time.deltaTime;
            yield return null;
        }

        elapsedtime = 0f;
        while (elapsedtime < falling_duration)
        {
            if (obj == null)
            {
                break;
            }
            obj.transform.Translate(Vector3.down * falling_speed * Time.deltaTime);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        remaining_blocks.Remove(obj);
        Destroy(obj);
        if (remaining_blocks.Count == 0)
        {
            game_clear = true;
        }
    }

    private IEnumerator CloudFall(GameObject obj)
    {
        //game_clear = true;
        SelectionBar.SetActive(false);
        float falling_speed = 0.5f;
        float falling_duration = 15f;
        float elapsedtime = 0f;
        while (elapsedtime < falling_duration)
        {
            if (obj == null)
            {
                break;
            }
            obj.transform.Translate(Vector3.down * falling_speed * Time.deltaTime);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }

    // player respawn
    private void Respawn()
    {
        Time.timeScale = 0f;
        ClearImageColor();
        BlockChoiceInitialize();
        foreach (GameObject obj in remaining_blocks)
        {
            Destroy(obj);
        }
        TerrainGeneration(terrain_radius, 5f);
        //Instantiate(tutorial_terrain_prefab);
        TerrainRefresh();
        MouseHidden();
        transform.position = new Vector3(0f, 8f, 0f);
        Camera.main.transform.position = transform.position + offset;
        Time.timeScale = 1f;
    }

    // parameters initialization
    private void BlockChoiceInitialize()
    {
        isJump = false;
        isSelecting = false;
        isHorizontalStatic = false;
        current_block_tag = "";
        hit_blocks = new List<GameObject>();
    }

    static void ListShuffle(GameObject[] numbers)
    {
        var rand = new System.Random();
        int n = numbers.Length;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            GameObject value = numbers[k];
            numbers[k] = numbers[n];
            numbers[n] = value;
        }
    }

    private void TerrainGeneration(int radius, float height)
    {

        for (int x = -radius; x < radius; x++)
        {
            for (int y = -radius; y < radius; y++)
            {
                if (Math.Abs(x) + Math.Abs(y) < radius)
                {
                    GameObject newblock = Instantiate(sample_cube, new Vector3((float)x, height - (Math.Abs(x) + Math.Abs(y)) * 0.2f, (float)y), Quaternion.identity);
                    //newblock.transform.SetParent(sample_cube.transform);
                    remaining_blocks.Add(newblock);
                }
            }
        }
        //PrefabUtility.SaveAsPrefabAsset(sample_cube, "Assets/Prefabs/level_terrain.prefab");
    }

    private void TerrainRefresh()
    {
        //GameObject[] blocks = GameObject.FindGameObjectsWithTag("PopBlocks");
        //remaining_blocks = new List<GameObject>(blocks);
        GameObject[] blocks = remaining_blocks.ToArray();
        ListShuffle(blocks);
        block_num = blocks.Length;
        Debug.Log(block_num.ToString() + " numbers of blocks in total");
        int ptr = 0;
        foreach (GameObject obj in blocks)
        {
            //Renderer renderer = obj.GetComponent<Renderer>();
            //Material material = renderer.material;
            //obj.GetComponent<Renderer>().material = block_material;
            //obj.GetComponent<Renderer>().material.shader = null;
            try
            {
                (UnityEngine.Color color, string color_tag) = block_colors[ptr];
                obj.GetComponent<Renderer>().material.SetColor("_BaseColor", color);
                obj.tag = color_tag;
                ptr = (ptr + 1) % 3;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
        }
    }

    private void Sunny()
    {
        RenderSettings.skybox.SetTexture("_Tex", sunnyCubeMap);
    }

    private void MouseActive()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void MouseHidden()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMenuClick()
    {
        game_state = "menuing";
        Time.timeScale = 0f;
        MouseActive();
        MenuPanel.SetActive(true);
    }

    public void OnTutorialClick()
    {
        WelcomePanel.SetActive(false);
        //terrain_prefab = tutorial_terrain_prefab;
        terrain_radius = tutorial_terrain_radius;
        Respawn(); // ----- need to be modified.
        MouseHidden();
        game_state = "tutorial";
    }

    public void OnNoTutorialClick()
    {
        WelcomePanel.SetActive(false);
        //terrain_prefab = tutorial_terrain_prefab;
        terrain_radius = level_terrain_radius;
        Respawn();
        MouseHidden();
        game_state = "playing";
    }

    public void OnTutorialFinished()
    {
        SceneManager.LoadScene(ThisScene);
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene(MainScene);
    }

    public void OnReplayClick()
    {
        SceneManager.LoadScene(ThisScene);
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(ThisScene);
        //Time.timeScale = 1f;
        //MenuPanel.SetActive(false);
        //game_state = "playing";
        //MouseHidden();
        //Respawn();
    }

    public void OnGuidanceClick()
    {
        MenuPanel.SetActive(false);
        GuidancePanel.SetActive(true);
    }

    public void OnResumeClick()
    {
        MenuPanel.SetActive(false);
        Time.timeScale = 1f;
        game_state = "playing";
        MouseHidden();
    }

    public void OnProceedClick()
    {
        GuidancePanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    private void UI_init()
    {
        proceed.onClick.AddListener(OnProceedClick);
        exit.onClick.AddListener(OnExitClick);
        resume.onClick.AddListener(OnResumeClick);
        restart.onClick.AddListener(OnRestartClick);
        guidance.onClick.AddListener(OnGuidanceClick);

        GuidancePanel.SetActive(false);
        MenuPanel.SetActive(false);
        GameClearPrompt.SetActive(false);
        WelcomePanel.SetActive(false);
        TutorialFinishedPanel.SetActive(false);

        selection_block_1 = GameObject.Find("block1");
        selection_block_2 = GameObject.Find("block2");
        selection_block_3 = GameObject.Find("block3");

        selection_block_1.SetActive(false);
        selection_block_2.SetActive(false);
        selection_block_3.SetActive(false);

        PuzzlePanel.SetActive(false);
        PuzzleGuidance.SetActive(false);

        game_state = "init";
    }

    private void ImageColoring(GameObject image, Color32 target_color)
    {
        image.GetComponent<Image>().color = target_color;
        image.SetActive(true);
    }

    private void ClearImageColor()
    {
        selection_block_1.SetActive(false);
        selection_block_2.SetActive(false);
        selection_block_3.SetActive(false);
    }

    private void PlayMusic(AudioClip music, bool looping = true)
    {
        audioSource.Stop();
        audioSource.clip = music;
        if (looping)
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }
        audioSource.Play();
    }

}
