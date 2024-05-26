using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    private bool key = false;
    private bool chest = false;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }
    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }
    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }
    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }

    public bool _LeftActive()
    {
        return _leftWall.activeSelf;
    }
    public bool _RightActive()
    {
        return _rightWall.activeSelf;
    }
    public bool _FrontActive()
    {
        return _frontWall.activeSelf;
    }
    public bool _BackActive()
    {
        return _backWall.activeSelf;
    }
    public bool getKey()
    {
        return key;
    }
    public void setKey()
    {
        key = true;
    }
    public bool getChest()
    {
        return chest;
    }
    public void setChest()
    {
        chest = true;
    }
}
