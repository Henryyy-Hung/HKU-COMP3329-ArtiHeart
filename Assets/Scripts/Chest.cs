using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using JetBrains.Annotations;

public class Chest : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    private bool bouncingBox = true;
    private UIManagerJoy UImanager;

    public GameObject puzzlePanelPrefab;

    private GameObject puzzlePanelInstance;

    public float rotationSpeed = 50.0f;

    private GameObject canvas;

    private GameObject playerCamera;

    private GameObject player;

    private AudioSource openSound;

    private Text keyPromptText; // 拖拽你的Text UI元素到这里
    public float displayDuration = 1f; // 提示显示的时间长度
    void Start()
    {
        openSound = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
        playerCamera = GameObject.FindWithTag("MainCamera");
        animator = GetComponent<Animator>();
        UImanager = FindFirstObjectByType<UIManagerJoy>();
        canvas = GameObject.FindWithTag("Canvas");
        // set the animation to bounce or not
        BounceBox(bouncingBox);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // 如果玩家靠近并且按下了指定的键，则打开宝箱
        if (CheckPlayerDistance() && Input.GetKeyDown(KeyCode.Space))
        {
            if (UImanager.GetKeyNumber() == 2)
            {
                openSound.Play();
                animator.Play("Open");
                ShowPuzzlePanel();
            }
            else
            {
                keyPromptText = GameObject.Find("Notification").GetComponent<Text>();
                //Debug.Log("Not enough key");
                StartCoroutine(DisplayKeyPrompt());
            }


        }
    }
    IEnumerator DisplayKeyPrompt()
    {
        keyPromptText.enabled = true;
        yield return new WaitForSeconds(displayDuration);
        keyPromptText.enabled = false;
    }

    public void BounceBox(bool bounceIt)
    {
        // flag the animator property "bounce" accordingly
        if (animator) animator.SetBool("bounce", bounceIt);
    }

    private bool CheckPlayerDistance()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        float distance = Vector3.Distance(transform.position, player.transform.position);
        return (distance < 0.5f);

    }

    public void ShowPuzzlePanel()
    {
        if (puzzlePanelInstance == null)
        {
            puzzlePanelInstance = Instantiate(puzzlePanelPrefab, canvas.transform);
        }
        playerCamera.GetComponent<CameraFollow2>().inactiveCamRotation();
        player.GetComponent<PlayerController>().FreezePlayer();
        puzzlePanelInstance.SetActive(true);
    }

    public void inactivePuzzlePanel()
    {
        puzzlePanelInstance.SetActive(false);
    }
}
