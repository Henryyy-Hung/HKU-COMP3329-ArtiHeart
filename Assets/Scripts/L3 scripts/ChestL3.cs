using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using JetBrains.Annotations;

public class ChestL3 : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    private bool bouncingBox = true;
    private UIManagerL3 UImanager;

    //public GameObject puzzlePanelPrefab;
    
    public GameObject puzzle_panel, PuzzleGuidance, ChestPrompting;
    //private AudioSource open_chest;

    public float rotationSpeed = 50.0f;
    public bool puzzling = false;

    //private GameObject canvas; 
    void Start()
    {
        //puzzle_panel = GameObject.Find("PuzzlePanel");
        //PuzzleGuidance = GameObject.Find("PuzzleGuidance");
        //ChestPrompting = GameObject.Find("ChestPrompting");
        ChestPrompting.SetActive(false);
        puzzle_panel.SetActive(false);
        PuzzleGuidance.SetActive(false);
        animator = GetComponent<Animator>();
        UImanager = FindFirstObjectByType<UIManagerL3>();
        //open_chest = GetComponent<AudioSource>();
        // set the animation to bounce or not
        BounceBox(bouncingBox);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // 如果玩家靠近并且按下了指定的键，则打开宝箱
        if (CheckPlayerDistance() && Input.GetKeyDown(KeyCode.F))
        {
            animator.Play("Open");
            puzzle_panel.SetActive(true);
            PuzzleGuidance.SetActive(true);
            puzzling = true;
        }
        else if (CheckPlayerDistance() && puzzling == false)
        {
            ChestPrompting.SetActive(true);
        }
        else
        {
            ChestPrompting.SetActive(false);
        }
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

}
