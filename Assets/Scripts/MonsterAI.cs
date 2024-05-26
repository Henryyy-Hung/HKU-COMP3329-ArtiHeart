using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI; // 确保引入UI命名空间

public class MonsterAI : MonoBehaviour
{

    private Animator ac;
    private float speed = 1f;

    private GameObject player;
    private MazeGeneration mazeGen;
    List<MazeCell> pathToPlayer;
    Dictionary<MazeCell, MazeCell> prevCellMap;

    private MazeCell lastPlayerCell;
    private MazeCell lastMonsterCell;

    private MazeCell currentMonsterCell;

    private MazeCell currentPlayerCell;

    //private AudioSource bgaudio;
    private AudioSource audioSource;


    public enum MonsterState
    {
        Tracking,
        Idle
    }

    private MonsterState currentState = MonsterState.Idle;

    private Text monsterAwakeTimerText; // UI Text组件的引用
    private float timeToToggleState = 10f; // 切换状态前的时间
    
    private bool playerFreeze = false;





    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        ac = GetComponent<Animator>();
        pathToPlayer = new List<MazeCell>();
        player = GameObject.FindWithTag("Player");
        
        mazeGen = Object.FindFirstObjectByType<MazeGeneration>();

        currentMonsterCell = GetCell(transform.position);
        currentPlayerCell = GetCell(player.transform.position);

        StartCoroutine(ToggleMonsterState());
        GameObject timerGameObject = GameObject.Find("Timer");
        monsterAwakeTimerText = timerGameObject.GetComponent<Text>();

    }

   
    void Update()
    {
        if (playerFreeze || player.GetComponent<PlayerController>().playerFreeze){return;}
        if (currentState == MonsterState.Tracking)
        {
            audioSource.pitch = 1.4f;
            // Tracking状态下的逻辑
            currentPlayerCell = GetCell(player.transform.position);
            ac.SetBool("IsTracking", true);

            // 检查游戏结束条件
            if (GetCell(transform.position) == GetCell(player.transform.position))
            {
                //Quit
                playerFreeze =true;
                audioSource.Pause();
                player.GetComponent<PlayerController>().PlayerDie();
            }
            // 仅当玩家或怪物移动到新单元格，或路径为空时，重新计算路径
            if (currentPlayerCell != lastPlayerCell || currentMonsterCell != lastMonsterCell || pathToPlayer.Count == 0)
            {
                findPathToPlayer(currentMonsterCell, currentPlayerCell);
                UpdatePathToPlayer();

            }
            TrackPlayer();

        }
        else if (currentState == MonsterState.Idle)
        {

            audioSource.pitch = 0.8f;
            // Idle状态下的逻辑
            // 可以选择不做任何事，即怪兽停止移动
            ac.SetBool("IsTracking", false);


        }


        // 如果怪物处于Idle状态，更新UI Text来显示剩余时间
        if (currentState == MonsterState.Idle)
        {
            timeToToggleState -= Time.deltaTime;
            monsterAwakeTimerText.text = "Monster awakes in: " + Mathf.Ceil(timeToToggleState).ToString() + "s";

        }
        else
        {
            monsterAwakeTimerText.text = "Monster Tracking!"; // 当怪物正在追踪时，不显示倒计时

        }

    }

    void TrackPlayer()
    {

        if (pathToPlayer.Count > 0)
        {
            MazeCell nextStep = pathToPlayer[0];
            FaceNextCell(nextStep);
            transform.position = Vector3.MoveTowards(transform.position, nextStep.transform.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, nextStep.transform.position) < 0.1f)
            {
                // 到达下一步，从路径中移除
                lastPlayerCell = currentPlayerCell;
                lastMonsterCell = currentMonsterCell;
                currentMonsterCell = nextStep;

                pathToPlayer.RemoveAt(0);

            }
        }
    }



    void findPathToPlayer(MazeCell start, MazeCell destination)
    {
        prevCellMap = new Dictionary<MazeCell, MazeCell>();
        Queue<MazeCell> planTravelCells = new Queue<MazeCell>();
        List<MazeCell> visitedCells = new List<MazeCell>();

        planTravelCells.Enqueue(start);
        visitedCells.Add(start);


        while (planTravelCells.Count != 0)
        {
            MazeCell current = planTravelCells.Dequeue();
            if (current == destination) { return; }
            foreach (MazeCell i in findNextCell(current, mazeGen._mazeGrid))
            {
                if (!visitedCells.Contains(i))
                {
                    visitedCells.Add(i);
                    planTravelCells.Enqueue(i);
                    prevCellMap[i] = current;
                }
            }
        }
    }
    MazeCell GetCell(Vector3 v)
    {

        //return mazeGen._mazeGrid[(int)v.x, (int)v.z];
        return mazeGen._mazeGrid[(int)(v.x + 0.5), (int)(v.z + 0.5)];
    }

    void UpdatePathToPlayer()
    {

        MazeCell current = GetCell(player.transform.position);
        while (prevCellMap.ContainsKey(current))
        {
            pathToPlayer.Add(current);
            current = prevCellMap[current];
        }
        pathToPlayer.Reverse();
    }



    List<MazeCell> findNextCell(MazeCell currentCell, MazeCell[,] _mazeGrid)
    {
        List<MazeCell> nextCells = new List<MazeCell>();
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;


        if (currentCell._LeftActive() == false)
        {
            nextCells.Add(_mazeGrid[x - 1, z]);
        }
        if (currentCell._RightActive() == false)
        {
            nextCells.Add(_mazeGrid[x + 1, z]);
        }
        if (currentCell._FrontActive() == false)
        {
            nextCells.Add(_mazeGrid[x, z + 1]);
        }
        if (currentCell._BackActive() == false)
        {
            nextCells.Add(_mazeGrid[x, z - 1]);
        }


        return nextCells;
    }


    public void QuitGame()
    {
        Application.Quit();
        // 如果在Unity编辑器中，可以添加以下行来模拟退出行为
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    IEnumerator ToggleMonsterState()
    {
        while (true) // 无限循环
        {
            // 等待10秒
            yield return new WaitForSeconds(10f);

            // 切换状态
            if (currentState == MonsterState.Tracking)
            {
                currentState = MonsterState.Idle;
            }
            else
            {
                currentState = MonsterState.Tracking;
            }
            timeToToggleState = 10f;

        }
    }

    void FaceNextCell(MazeCell nextCell)
    {
        Vector3 directionToFace = nextCell.transform.position - transform.position;
        // Ensure that the y-component is zero to keep the monster facing horizontally
        directionToFace.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }



}
